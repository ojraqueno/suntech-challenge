using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Suntech.Functions.Data;
using Suntech.Functions.EventGrid;
using Suntech.Functions.ServiceBus;
using System;

[assembly: FunctionsStartup(typeof(Suntech.Functions.Startup))]
namespace Suntech.Functions;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddSuntechServiceBus(options =>
        {
            options.ConnectionString = Environment.GetEnvironmentVariable("sb-connectionstring");
        });

        builder.Services.AddSuntechCosmosDb(options =>
        {
            options.Endpoint = Environment.GetEnvironmentVariable("cosmos-endpoint");
            options.Key = Environment.GetEnvironmentVariable("cosmos-key");
            options.Database = "default";
        });

        builder.Services.AddSuntechEventGrid(options =>
        {
            options.Endpoint = Environment.GetEnvironmentVariable("evgt-customersaved-endpoint");
            options.Key = Environment.GetEnvironmentVariable("evgt-customersaved-key");
        });
    }
}
