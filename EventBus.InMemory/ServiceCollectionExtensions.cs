using EventBus.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace EventBus.InMemory;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEventBus(
        this IServiceCollection services)
    {
        services.AddSingleton<IEventBus, InMemoryEventBus>();
        return services;
    }
}