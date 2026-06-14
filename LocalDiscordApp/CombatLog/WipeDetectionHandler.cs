using EventBus.Abstractions;
using WowCombatLog.Events;

namespace LocalDiscordApp.CombatLog;

public class WipeDetectionHandler : IEventHandler<WowDeathEvent>
{
    private readonly IEventBus _eventBus;
    private readonly List<WowDeathEvent> _recentDeaths = new();

    private static readonly TimeSpan Window = TimeSpan.FromSeconds(5);
    private const int DeathThreshold = 5;

    public WipeDetectionHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task HandleAsync(
        WowDeathEvent deathEvent,
        CancellationToken cancellationToken = default)
    {
        var now = DateTimeOffset.UtcNow;

        _recentDeaths.Add(deathEvent);

        _recentDeaths.RemoveAll(death =>
            now - death.Timestamp > Window);

        if (_recentDeaths.Count < DeathThreshold)
            return;

        var deadPlayers = _recentDeaths
            .Select(death => death.PlayerName)
            .Distinct()
            .ToList();

        await _eventBus.PublishAsync(
            new WowWipeEvent(
                deadPlayers,
                now),
            cancellationToken);

        _recentDeaths.Clear();
    }
}