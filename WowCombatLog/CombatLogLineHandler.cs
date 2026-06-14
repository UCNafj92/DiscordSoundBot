using EventBus.Abstractions;
using FileReader.Abstractions.Events;
using WowCombatLog.Parsing;

namespace WowCombatLog;

public class CombatLogLineHandler : IEventHandler<LineReadFromFileEvent>
{
    private readonly IEventBus _eventBus;
    private readonly IEnumerable<ICombatLogEventParser> _parsers;

    public CombatLogLineHandler(
        IEventBus eventBus,
        IEnumerable<ICombatLogEventParser> parsers)
    {
        _eventBus = eventBus;
        _parsers = parsers;
    }

    public void Subscribe()
    {
        _eventBus.Subscribe<LineReadFromFileEvent>(this);
    }

    public async Task HandleAsync(
        LineReadFromFileEvent lineEvent,
        CancellationToken cancellationToken = default)
    {
        foreach (var parser in _parsers)
        {
            var parsedEvent = parser.TryParse(
                lineEvent.Line,
                lineEvent.Timestamp);

            if (parsedEvent is null)
                continue;

            await _eventBus.PublishAsync(
                parsedEvent,
                cancellationToken);
        }
    }
}