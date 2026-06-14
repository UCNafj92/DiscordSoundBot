using EventBus.Abstractions;
using FileReader.Abstractions;
using FileReader.Abstractions.Events;

namespace FileReader.Windows;

public class FileWatcher : IFileWatcher
{
    private readonly IEventBus _eventBus;
    private readonly ILineFilter _lineFilter;
    private bool _isRunning;

    public FileWatcher(
        IEventBus eventBus,
        ILineFilter lineFilter)
    {
        _eventBus = eventBus;
        _lineFilter = lineFilter;
    }

    public async Task StartAsync(
        string filePath,
        CancellationToken cancellationToken = default)
    {
        _isRunning = true;

        using var fileStream = new FileStream(
            filePath,
            FileMode.Open,
            FileAccess.Read,
            FileShare.ReadWrite);

        using var reader = new StreamReader(fileStream);

        fileStream.Seek(0, SeekOrigin.End);

        while (_isRunning && !cancellationToken.IsCancellationRequested)
        {
            var line = await reader.ReadLineAsync();

            if (line is null)
            {
                await Task.Delay(500, cancellationToken);
                continue;
            }

            if (!_lineFilter.ShouldPublish(line))
                continue;

            await _eventBus.PublishAsync(
                new LineReadFromFileEvent(
                    filePath,
                    line,
                    DateTimeOffset.UtcNow),
                cancellationToken);
        }
    }

    public void Stop()
    {
        _isRunning = false;
    }
}