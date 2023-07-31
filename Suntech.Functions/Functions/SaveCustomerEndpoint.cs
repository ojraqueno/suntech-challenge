using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Suntech.Functions.Model;
using Suntech.Functions.ServiceBus;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Suntech.Functions.Functions;

public class SaveCustomerEndpoint
{
    private readonly ISuntechServiceBusClient _serviceBusClient;

    public SaveCustomerEndpoint(ISuntechServiceBusClient serviceBusClient)
    {
        _serviceBusClient = serviceBusClient;
    }

    [FunctionName(nameof(SaveCustomerEndpoint))]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
        ILogger log)
    {
        log.LogInformation("C# HTTP trigger function processed a request.");

        try
        {
            var customerRaw = await new StreamReader(req.Body).ReadToEndAsync();
            log.LogInformation($"Got request body {customerRaw}");

            var customer = JsonSerializer.Deserialize<Customer>(customerRaw);
            customer.Id = Guid.NewGuid().ToString();

            var customerJson = JsonSerializer.Serialize(customer);
            log.LogInformation($"Got customer JSON {customerJson}");

            await _serviceBusClient.QueueCustomer(customerJson);

            return new OkObjectResult($"Customer {customer.Id} queued");
        }
        catch (Exception ex)
        {
            log.LogError(ex, ex.Message);
            return new ObjectResult(ex.Message) { StatusCode = 500 };
        }
    }
}
