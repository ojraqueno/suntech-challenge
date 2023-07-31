using Microsoft.Extensions.DependencyInjection;
using System;

namespace Suntech.Functions.EventGrid;

internal static class IServiceCollectionExtensions
{
    internal static IServiceCollection AddSuntechEventGrid(this IServiceCollection services, Action<SuntechEventGridOptions> configure)
    {
        var options = new SuntechEventGridOptions();
        configure(options);
        if (!options.IsValid()) throw new InvalidOperationException($"Invalid {nameof(SuntechEventGridOptions)} detected.");

        services.AddSingleton(options);
        services.AddSingleton<ISuntechEventGridClient, SuntechEventGridClient>();

        return services;
    }
}
