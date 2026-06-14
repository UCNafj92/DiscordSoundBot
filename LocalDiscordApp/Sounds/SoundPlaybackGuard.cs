using System;
using System.Collections.Generic;
using System.Text;

namespace LocalDiscordApp.Sounds;

public class SoundPlaybackGuard
{
    private readonly Dictionary<string, DateTimeOffset> _lastEventTimes = new();

    public bool CanPlaySliding(string soundKey, TimeSpan cooldown)
    {
        var now = DateTimeOffset.UtcNow;

        if (_lastEventTimes.TryGetValue(soundKey, out var lastEventTime))
        {
            _lastEventTimes[soundKey] = now;

            return now - lastEventTime >= cooldown;
        }

        _lastEventTimes[soundKey] = now;
        return true;
    }
}
