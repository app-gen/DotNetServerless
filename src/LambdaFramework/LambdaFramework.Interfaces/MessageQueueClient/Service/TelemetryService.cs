using System.Diagnostics;
using System.Diagnostics.Metrics;
//using OpenTelemetry;

namespace MsgQueue.Client.Service;

public class TelemetryService
{
    private readonly Meter _meter;
    private readonly Counter<int> _messageProcessedCounter;
    private readonly Counter<int> _messageErrorCounter;
    private readonly Histogram<double> _messageProcessingDuration;
    private static readonly ActivitySource _activitySource = new("MessageProcessingService");

    public TelemetryService()
    {
        _meter = new Meter("MessageProcessingService", "1.0.0");
        
        _messageProcessedCounter = _meter.CreateCounter<int>(
            "message_processed_total",
            description: "Number of messages processed");
            
        _messageErrorCounter = _meter.CreateCounter<int>(
            "message_errors_total",
            description: "Number of message processing errors");
            
        _messageProcessingDuration = _meter.CreateHistogram<double>(
            "message_processing_duration_seconds",
            unit: "s",
            description: "Message processing duration");
    }

    public Activity? StartProcessingActivity(string topicId, string messageId)
    {
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        var activity = _activitySource.StartActivity(
            "ProcessMessage",
            ActivityKind.Internal,
            parentContext: default,
            tags: new[] {
                new KeyValuePair<string, object>("topic_id", topicId),
                new KeyValuePair<string, object>("message_id", messageId)
            });
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

        return activity;
    }

    public void RecordMessageProcessed(string topicId)
    {
        _messageProcessedCounter.Add(1, new KeyValuePair<string, object?>("topic_id", topicId));
    }

    public void RecordMessageError(string topicId, string errorType)
    {
        _messageErrorCounter.Add(1, new[] {
            new KeyValuePair<string, object?>("topic_id", topicId),
            new KeyValuePair<string, object?>("error_type", errorType)
        });
    }

    public void RecordProcessingDuration(string topicId, double durationSeconds)
    {
        _messageProcessingDuration.Record(durationSeconds, new KeyValuePair<string, object?>("topic_id", topicId));
    }
}
