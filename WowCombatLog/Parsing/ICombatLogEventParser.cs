using EventBus.Abstractions;

namespace WowCombatLog.Parsing;

public interface ICombatLogEventParser
{
    IEvent? TryParse(string line, DateTimeOffset timestamp);
}