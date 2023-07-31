using Microsoft.Extensions.DependencyInjection;
using System;

namespace Suntech.Functions.ServiceBus;

internal static class IServiceCollectionExtensions
{
    internal static IServiceCollection AddSuntechServiceBus(this IServiceCollection services, Action<SuntechServiceBusClientOptions> configure)
    {
        var options = new SuntechServiceBusClientOptions();
        configure(options);
        if (!options.IsValid()) throw new InvalidOperationException($"Invalid {nameof(SuntechServiceBusClientOptions)} detected.");

        services.AddSingleton(options);
        services.AddSingleton<ISuntechServiceBusClient, SuntechServiceBusClient>();

        return services;
    }
}
