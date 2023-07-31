using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Suntech.Functions.EventGrid;
using Suntech.Functions.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Suntech.Functions.Functions;

public class CustomerSaved
{
    private readonly ISuntechEventGridClient _eventGridClient;

    public CustomerSaved(ISuntechEventGridClient eventGridClient)
    {
        _eventGridClient = eventGridClient;
    }

    [FunctionName(nameof(CustomerSaved))]
    public async Task Run([CosmosDBTrigger(
        databaseName: "default",
        containerName: "customers",
        Connection = "cosmos-connectionstring",
        CreateLeaseContainerIfNotExists = true,
        LeaseContainerName = "leases")]IReadOnlyList<Customer> customers,
        ILogger log)
    {
        log.LogInformation("C# CosmosDBTrigger trigger function processed customers");

        if (customers == null || customers.Count == 0)
        {
            log.LogInformation("No customers found");
            return;
        }

        log.LogInformation($"{customers.Count} customers saved.");
        log.LogInformation("First customer id " + customers[0].Id);

        try
        {
            await _eventGridClient.PublishCustomerSaved(customers[0]);
        }
        catch (Exception ex)
        {
            log.LogError(ex, ex.Message);
        }

        log.LogInformation("Event grid event publisehd");
    }
}
