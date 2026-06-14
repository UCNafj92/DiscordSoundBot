using EventBus.Abstractions;
using WowCombatLog.Events;

namespace WowCombatLog.Parsing;

public class PlayerResurrectParser : ICombatLogEventParser
{
    public IEvent? TryParse(string line, DateTimeOffset timestamp)
    {
        if (!line.Contains("SPELL_RESURRECT"))
            return null;

        var parts = CombatLogTokenizer.Tokenize(line);

        if (parts.Count < 11)
            return null;

        var sourceGuid = parts[1];
        var targetGuid = parts[5];

        if (!sourceGuid.StartsWith("Player-"))
            return null;

        if (!targetGuid.StartsWith("Player-"))
            return null;

        return new WowResurrectEvent(
            parts[2],
            parts[6],
            parts[10],
            line,
            timestamp);
    }
}