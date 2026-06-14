using EventBus.Abstractions;
using LocalDiscordApp.CombatLog;
using LocalDiscordApp.Service;
using LocalDiscordApp.Sounds;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WowCombatLog.Events;

namespace LocalDiscordApp;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLocalDiscordApp(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<SoundOptions>(
            configuration.GetSection("Sounds"));

        services.Configure<AudioOptions>(
            configuration.GetSection("Audio"));

        services.AddSingleton<SoundLibrary>();
        services.AddSingleton<IDiscordVoiceService, DiscordVoiceService>();
        services.AddSingleton<SoundPlaybackGuard>();
        services.AddSingleton<IEventHandler<WowDeathEvent>, CombatLogDeathHandler>();
        services.AddSingleton<IEventHandler<WowDeathEvent>, WipeDetectionHandler>();

        services.AddSingleton<IEventHandler<WowResurrectEvent>, CombatLogResurrectHandler>();

        services.AddSingleton<IEventHandler<WowWipeEvent>, CombatLogWipeHandler>();

        return services;
    }

    public static IServiceProvider UseLocalDiscordApp(
        this IServiceProvider services)
    {
        var eventBus = services.GetRequiredService<IEventBus>();

        foreach (var handler in services.GetServices<IEventHandler<WowDeathEvent>>())
            eventBus.Subscribe(handler);

        foreach (var handler in services.GetServices<IEventHandler<WowResurrectEvent>>())
            eventBus.Subscribe(handler);

        foreach (var handler in services.GetServices<IEventHandler<WowWipeEvent>>())
            eventBus.Subscribe(handler);

        return services;
    }
}