using System;
using System.Collections.Generic;
using System.Text;

namespace FileReader.Abstractions
{
    public interface IFileWatcher
    {
        Task StartAsync(
            string filePath,
            CancellationToken cancellationToken = default);

        void Stop();
    }
}
