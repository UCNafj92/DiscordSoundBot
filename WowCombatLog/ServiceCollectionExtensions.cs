using EventBus.Abstractions;
using FileReader.Abstractions;
using FileReader.Abstractions.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WowCombatLog.Parsing;
using WowCombatLog.Rules;

namespace WowCombatLog;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWowCombatLog(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<CombatLogOptions>(
            configuration.GetSection("CombatLog"));

        services.AddSingleton<ICombatLogEventParser, PlayerDeathParser>();
        services.AddSingleton<ICombatLogEventParser, PlayerResurrectParser>();
        services.AddSingleton<CombatLogLineHandler>();
        services.AddSingleton<CombatLogFileResolver>();
        services.AddSingleton<CombatLogMonitorService>();
        services.AddSingleton<ICombatLogLineRule, PlayerDeathLineRule>();
        services.AddSingleton<ICombatLogLineRule, CombatResurrectionLineRule>();
        services.AddSingleton<ILineFilter, CombatLogLineFilter>();

        return services;
    }

    public static IServiceProvider UseWowCombatLog(
        this IServiceProvider services)
    {
        var eventBus = services.GetRequiredService<IEventBus>();

        eventBus.Subscribe<LineReadFromFileEvent>(
            services.GetRequiredService<CombatLogLineHandler>());

        return services;
    }
}