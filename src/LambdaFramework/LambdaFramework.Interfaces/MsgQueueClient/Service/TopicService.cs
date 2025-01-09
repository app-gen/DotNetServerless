using Microsoft.Extensions.Configuration;

namespace MsgQueue.Client.Service;

public class TopicService : ITopicService
{

    private readonly List<ITopicConfig> _topics;

    public TopicService(IConfiguration configuration)
    {
        var topicConfigList = configuration
            .GetSection("TopicConfigs")
            .Get<TopicConfigList>();

        _topics = topicConfigList?.Topics.Cast<ITopicConfig>().ToList() ?? new List<ITopicConfig>();
    }

    public ITopicConfig? GetTopicConfig(string id)
    {
        return _topics.FirstOrDefault(t => t.TopicId == id);
    }

}

/*
 * {
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*",
  "TopicConfigs": {
    "Topics": [
      {
        "TopicId": "order-processing",
        "TopicName": "Order Processing Queue",
        "MaxTasks": 50,
        "WaitTimeInSecondsIfEmpty": 30,
        "CommandName": "ProcessOrder",
        "ActionName": "ProcessOrderAction",
        "TenantName": "RetailStore",
        "CommandVersion": "1.0",
        "Active": true,
        "StartTimeUtc": "2024-01-01T00:00:00Z",
        "EndTimeUtc": null,
        "Comments": "Handles all incoming retail orders"
      },
      {
        "TopicId": "email-notifications",
        "TopicName": "Email Notification Queue",
        "MaxTasks": 100,
        "WaitTimeInSecondsIfEmpty": 15,
        "CommandName": "SendEmail",
        "ActionName": "SendEmailAction",
        "TenantName": "Communications",
        "CommandVersion": "2.1",
        "Active": true,
        "StartTimeUtc": "2024-01-01T00:00:00Z",
        "EndTimeUtc": null,
        "Comments": "Handles all outgoing email notifications"
      },
      {
        "TopicId": "payment-processing",
        "TopicName": "Payment Processing Queue",
        "MaxTasks": 25,
        "WaitTimeInSecondsIfEmpty": 10,
        "CommandName": "ProcessPayment",
        "ActionName": "ProcessPaymentAction",
        "TenantName": "Finance",
        "CommandVersion": "1.2",
        "Active": true,
        "StartTimeUtc": "2024-01-01T00:00:00Z",
        "EndTimeUtc": null,
        "Comments": "Handles payment processing and verification"
      },
      {
        "TopicId": "legacy-sync",
        "TopicName": "Legacy System Sync",
        "MaxTasks": 10,
        "WaitTimeInSecondsIfEmpty": 60,
        "CommandName": "SyncLegacy",
        "ActionName": "SyncLegacyAction",
        "TenantName": "Integration",
        "CommandVersion": "1.0",
        "Active": false,
        "StartTimeUtc": "2023-01-01T00:00:00Z",
        "EndTimeUtc": "2024-12-31T23:59:59Z",
        "Comments": "Synchronization with legacy system - scheduled for decommission"
      },
      {
        "TopicId": "inventory-updates",
        "TopicName": "Inventory Management Queue",
        "MaxTasks": 75,
        "WaitTimeInSecondsIfEmpty": 20,
        "CommandName": "UpdateInventory",
        "ActionName": "UpdateInventoryAction",
        "TenantName": "Warehouse",
        "CommandVersion": "3.0",
        "Active": true,
        "StartTimeUtc": "2024-01-01T00:00:00Z",
        "EndTimeUtc": null,
        "Comments": "Real-time inventory updates and synchronization"
      }
    ]
  }
}
 */