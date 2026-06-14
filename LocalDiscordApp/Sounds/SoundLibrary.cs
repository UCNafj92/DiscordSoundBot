using System;
using System.Collections.Generic;
using System.Text;

namespace LocalDiscordApp.Sounds
{
    using Microsoft.Extensions.Options;

    public class SoundLibrary
    {
        private readonly SoundOptions _options;

        public SoundLibrary(
            IOptions<SoundOptions> options)
        {
            _options = options.Value;
        }

        public string GetSoundPath(string soundKey)
        {
            if (!_options.Mappings.TryGetValue(
                soundKey,
                out var fileName))
            {
                throw new KeyNotFoundException(
                    $"Sound '{soundKey}' not configured.");
            }

            return Path.Combine(
                _options.FolderPath,
                fileName);
        }
    }
}
