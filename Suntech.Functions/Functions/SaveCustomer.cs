using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Suntech.Functions.Data;
using Suntech.Functions.Model;
using System;
using System.Threading.Tasks;

namespace Suntech.Functions.Functions;

public class SaveCustomer
{
    private readonly ISuntechCosmosDbClient _cosmosDbClient;

    public SaveCustomer(ISuntechCosmosDbClient cosmosDbClient)
    {
        _cosmosDbClient = cosmosDbClient;
    }

    [FunctionName(nameof(SaveCustomer))]
    public async Task Run([ServiceBusTrigger("save-customer", Connection = "sb-connectionstring")] Customer customer, ILogger log)
    {
        log.LogInformation($"C# ServiceBus queue trigger function processed message: {customer}");

        try
        {
            await _cosmosDbClient.CreateCustomer(customer);
        }
        catch (Exception ex)
        {
            log.LogError(ex, ex.Message);
        }

        log.LogInformation("Customer saved");
    }
}
