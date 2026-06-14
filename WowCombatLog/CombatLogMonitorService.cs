using FileReader.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace WowCombatLog
{
    public class CombatLogMonitorService
    {
        private readonly IFileWatcher _fileWatcher;
        private readonly CombatLogFileResolver _resolver;
        private readonly CombatLogOptions _options;
        private CancellationTokenSource? _cts;

        public CombatLogMonitorService(
            IFileWatcher fileWatcher,
            CombatLogFileResolver resolver,
            IOptions<CombatLogOptions> options)
        {
            _fileWatcher = fileWatcher;
            _resolver = resolver;
            _options = options.Value;
        }

        public Task StartAsync()
        {
            var filePath = _resolver.GetLatestCombatLogPath(_options.FolderPath);
            _cts = new CancellationTokenSource();

            return Task.Run(() =>
                _fileWatcher.StartAsync(filePath, _cts.Token));
        }

        public void Stop()
        {
            _cts?.Cancel();
            _fileWatcher.Stop();
        }
    }
}
