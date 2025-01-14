using Microsoft.Extensions.Logging;
using System.Text.Json;



using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;

using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Microsoft.Extensions.Options;
using MessageAttributeValue = Amazon.SQS.Model.MessageAttributeValue;




namespace MsgQueue.Server;

public class SnsConfig
{
    public string AwsAccessKey { get; set; }
    public string AwsSecretKey { get; set; }
    public string Region { get; set; }
    public int RetryAttempts { get; set; } = 3;
    public int RetryDelayMs { get; set; } = 1000;
    public string QueueUrlPrefix { get; set; }
    public Dictionary<string, string> TopicToQueueMap { get; set; }
}


public class SnsQueueProvider : IQueueProvider, IDisposable
{
    private readonly IAmazonSQS _snsClient;
    private readonly ILogger<SnsQueueProvider> _logger;
    private readonly Dictionary<string, string> _queueUrlCache;
    private readonly SnsConfig _config;

    public QueueProviderType ProviderType => QueueProviderType.Sns;



    public SnsQueueProvider(Dictionary<string, string> settings, ILogger<SnsQueueProvider> logger)
    {
        _logger = logger;
        _queueUrlCache = new Dictionary<string, string>();

        _config = new SnsConfig
        {
            AwsAccessKey = settings["AwsAccessKey"],
            AwsSecretKey = settings["AwsSecretKey"],
            Region = settings["AwsRegion"],
            QueueUrlPrefix = settings.GetValueOrDefault("QueueUrlPrefix", "https://sns.{region}.amazonaws.com/{account}/"),
            RetryAttempts = int.Parse(settings.GetValueOrDefault("RetryAttempts", "3")),
            RetryDelayMs = int.Parse(settings.GetValueOrDefault("RetryDelayMs", "1000")),
            TopicToQueueMap = settings.ContainsKey("TopicToQueueMap")
                ? JsonSerializer.Deserialize<Dictionary<string, string>>(settings["TopicToQueueMap"])
                : new Dictionary<string, string>()
        };

        var snsConfig = new AmazonSQSConfig
        {
            RegionEndpoint = RegionEndpoint.GetBySystemName(_config.Region)
        };

        _snsClient = new AmazonSQSClient(
            _config.AwsAccessKey,
            _config.AwsSecretKey,
            snsConfig
        );

    }

    public async Task SendMessageAsync(IMessageQueueEntry message)
    {
        try
        {
            var queueUrl = await GetQueueUrlForTopic(message.TopicId);
            var messageBody = CreateMessageBody(message);
            var messageAttributes = CreateMessageAttributes(message);

            var sendMessageRequest = new SendMessageRequest
            {
                QueueUrl = queueUrl,
                MessageBody = messageBody,
                MessageAttributes = messageAttributes
            };

            // Add delay if specified
            if (message.ExecutionTimeUtc.HasValue)
            {
                var delay = (int)(message.ExecutionTimeUtc.Value - DateTime.UtcNow).TotalSeconds;
                if (delay > 0 && delay <= 900) // sns allows max delay of 15 minutes
                {
                    sendMessageRequest.DelaySeconds = delay;
                }
            }

            await SendMessageWithRetryAsync(sendMessageRequest);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send message to sns. TopicId: {TopicId}, MessageId: {MessageId}",
                message.TopicId, message.MessageId);
            throw;
        }
    }

    private async Task SendMessageWithRetryAsync(SendMessageRequest request)
    {
        var attempts = 0;
        while (true)
        {
            try
            {
                attempts++;
                var response = await _snsClient.SendMessageAsync(request);
                _logger.LogInformation("Message sent to sns. MessageId: {MessageId}", response.MessageId);
                return;
            }
            catch (Exception ex) when (attempts < _config.RetryAttempts)
            {
                _logger.LogWarning(ex, "Failed to send message to sns. Attempt {Attempt} of {MaxAttempts}",
                    attempts, _config.RetryAttempts);
                await Task.Delay(_config.RetryDelayMs * attempts);
            }
        }
    }

    private async Task<string> GetQueueUrlForTopic(string topicId)
    {
        if (_queueUrlCache.TryGetValue(topicId, out var cachedUrl))
        {
            return cachedUrl;
        }

        string queueName;
        if (_config.TopicToQueueMap.TryGetValue(topicId, out var mappedQueue))
        {
            queueName = mappedQueue;
        }
        else
        {
            queueName = topicId.Replace(".", "-").ToLower(); // sns naming conventions
        }

        try
        {
            var response = await _snsClient.GetQueueUrlAsync(queueName);
            var queueUrl = response.QueueUrl;
            _queueUrlCache[topicId] = queueUrl;
            return queueUrl;
        }
        catch (QueueDoesNotExistException)
        {
            _logger.LogError("sns Queue not found for topic {TopicId}", topicId);
            throw new InvalidOperationException($"Queue not found for topic {topicId}");
        }
    }

    private string CreateMessageBody(IMessageQueueEntry message)
    {
        var messageBody = new
        {
            message.MessageId,
            message.TopicId,
            message.MessageBody,
            message.User,
            message.System,
            message.CommandName,
            message.ActionName,
            message.CommandVersion,
            message.TenantName,
           // Context = message.Context,
           // ExtraData = message.ExtraData,
            CreatedDate = DateTime.UtcNow
        };

        return JsonSerializer.Serialize(messageBody);
    }

    private Dictionary<string, MessageAttributeValue> CreateMessageAttributes(IMessageQueueEntry message)
    {
        var attributes = new Dictionary<string, MessageAttributeValue>();

        if (message.MessagePriority.HasValue)
        {
            attributes["Priority"] = new MessageAttributeValue
            {
                DataType = "Number",
                StringValue = message.MessagePriority.ToString()
            };
        }


        if (!string.IsNullOrEmpty(message.System))
        {
            attributes["System"] = new MessageAttributeValue
            {
                DataType = "String",
                StringValue = message.System
            };
        }

        if (!string.IsNullOrEmpty(message.TenantName))
        {
            attributes["Tenant"] = new MessageAttributeValue
            {
                DataType = "String",
                StringValue = message.TenantName
            };
        }

        if (!string.IsNullOrEmpty(message.CommandName))
        {
            attributes["CommandName"] = new MessageAttributeValue
            {
                DataType = "String",
                StringValue = message.CommandName
            };
        }

        if (!string.IsNullOrEmpty(message.ActionName))
        {
            attributes["ActionName"] = new MessageAttributeValue
            {
                DataType = "String",
                StringValue = message.ActionName
            };
        }

        if (!string.IsNullOrEmpty(message.CommandVersion))
        {
            attributes["CommandVersion"] = new MessageAttributeValue
            {
                DataType = "String",
                StringValue = message.CommandVersion
            };
        }




        return attributes;
    }

    public async Task<bool> IsHealthyAsync()
    {
        try
        {
            // List queues as a simple health check
            var response = await _snsClient.ListQueuesAsync(new ListQueuesRequest
            {
                MaxResults = 1
            });
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "sns health check failed");
            return false;
        }
    }

    public void Dispose()
    {
        _snsClient?.Dispose();
    }
}
