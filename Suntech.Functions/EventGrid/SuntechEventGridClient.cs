using Azure;
using Azure.Messaging.EventGrid;
using Microsoft.Extensions.Logging;
using Suntech.Functions.Model;
using System;
using System.Threading.Tasks;

namespace Suntech.Functions.EventGrid;

public interface ISuntechEventGridClient
{
    Task PublishInfo(string eventType, object data);
}

public class SuntechEventGridClient : ISuntechEventGridClient
{
    private readonly ILogger<ISuntechEventGridClient> _logger;
    private readonly EventGridPublisherClient _publisher;

    public SuntechEventGridClient(ILogger<ISuntechEventGridClient> logger, SuntechEventGridOptions options)
    {
        _logger = logger;
        _publisher = new EventGridPublisherClient(new Uri(options.Endpoint), new AzureKeyCredential(options.Key));
    }

    public async Task PublishInfo(string eventType, object data)
    {
        try
        {
            _logger.LogInformation($"Publishing event {eventType}");

            var egEvent = new EventGridEvent(eventType, "Informational", "1.0", data);
            await _publisher.SendEventAsync(egEvent);

            _logger.LogInformation("Publish success");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}

internal static class ISuntechEventGridClientExtensions
{
    internal static Task PublishCustomerSaved(this ISuntechEventGridClient client, Customer customer) => client.PublishInfo("Customer Saved", $"Customer {customer.Id} saved");
}