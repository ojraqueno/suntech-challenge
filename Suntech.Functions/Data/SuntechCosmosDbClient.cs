using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Suntech.Functions.Model;
using System;
using System.Threading.Tasks;

namespace Suntech.Functions.Data;

public interface ISuntechCosmosDbClient
{
    Task CreateItem<T>(string containerName, T item);
}

public class SuntechCosmosDbClient : ISuntechCosmosDbClient
{
    private readonly ILogger<SuntechCosmosDbClient> _logger;
    private readonly SuntechCosmosDbOptions _options;

    public SuntechCosmosDbClient(ILogger<SuntechCosmosDbClient> logger, SuntechCosmosDbOptions options)
    {
        _logger = logger;
        _options = options;
    }

    public async Task CreateItem<T>(string containerName, T item)
    {
        try
        {
            using (var client = new CosmosClient(_options.Endpoint, _options.Key))
            {
                var database = client.GetDatabase(_options.Database);
                var container = database.GetContainer(containerName);

                _logger.LogInformation($"Creating {typeof(T).Name}");

                await container.CreateItemAsync(item);

                _logger.LogInformation("Create success");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}

internal static class ICosmosDbClientExtensions
{
    internal static async Task CreateCustomer(this ISuntechCosmosDbClient client, Customer customer) => await client.CreateItem("customers", customer);
}