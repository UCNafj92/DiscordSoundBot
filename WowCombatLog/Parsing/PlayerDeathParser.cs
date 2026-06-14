using EventBus.Abstractions;
using WowCombatLog.Events;

namespace WowCombatLog.Parsing;

public class PlayerDeathParser : ICombatLogEventParser
{
    public IEvent? TryParse(string line, DateTimeOffset timestamp)
    {
        if (!line.Contains("UNIT_DIED"))
            return null;

        var parts = CombatLogTokenizer.Tokenize(line);

        if (parts.Count < 7)
            return null;

        var targetGuid = parts[5];

        if (!targetGuid.StartsWith("Player-"))
            return null;

        return new WowDeathEvent(
            parts[6],
            line,
            timestamp);
    }
}