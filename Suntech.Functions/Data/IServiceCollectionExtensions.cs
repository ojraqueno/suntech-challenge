using Microsoft.Extensions.DependencyInjection;
using System;

namespace Suntech.Functions.Data;

internal static class IServiceCollectionExtensions
{
    internal static IServiceCollection AddSuntechCosmosDb(this IServiceCollection services, Action<SuntechCosmosDbOptions> configure)
    {
        var options = new SuntechCosmosDbOptions();
        configure(options);
        if (!options.IsValid()) throw new InvalidOperationException($"Invalid {nameof(SuntechCosmosDbOptions)} detected.");

        services.AddSingleton(options);
        services.AddSingleton<ISuntechCosmosDbClient, SuntechCosmosDbClient>();

        return services;
    }
}
