using EventBus.Abstractions;

namespace WowCombatLog.Events;

public record WowWipeEvent(
    IReadOnlyList<string> DeadPlayers,
    DateTimeOffset Timestamp
) : IEvent;