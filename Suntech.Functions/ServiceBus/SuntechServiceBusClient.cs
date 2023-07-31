using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Suntech.Functions.ServiceBus;

public interface ISuntechServiceBusClient
{
    Task Queue(string queueName, string message);
}

public class SuntechServiceBusClient : ISuntechServiceBusClient
{
    private readonly ILogger<SuntechServiceBusClient> _logger;
    private readonly ServiceBusClient _serviceBusClient;

    public SuntechServiceBusClient(ILogger<SuntechServiceBusClient> logger, SuntechServiceBusClientOptions options)
    {
        _logger = logger;
        _serviceBusClient = new ServiceBusClient(options.ConnectionString, new ServiceBusClientOptions()
        {
            TransportType = ServiceBusTransportType.AmqpWebSockets
        });
    }

    public async Task Queue(string queueName, string message)
    {
        try
        {
            var sender = _serviceBusClient.CreateSender(queueName);

            _logger.LogInformation($"Queueing to {queueName}");

            await sender.SendMessageAsync(new ServiceBusMessage(message));

            _logger.LogInformation("Queueing success");
        }
        catch (ServiceBusException ex)
        {
            _logger.LogError(ex, $"{nameof(ServiceBusException)} encountered: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}

internal static class ISuntechServiceBusClientExtensions
{
    internal static async Task QueueCustomer(this ISuntechServiceBusClient client, string customerJson) => await client.Queue("save-customer", customerJson);
}