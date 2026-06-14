using EventBus.Abstractions;
using LocalDiscordApp.Service;
using LocalDiscordApp.Sounds;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocalDiscordApp.CombatLog
{
    public abstract class CombatLogSoundHandler<TEvent> : IEventHandler<TEvent>
    where TEvent : IEvent
    {
        private readonly IDiscordVoiceService _voiceService;
        private readonly SoundPlaybackGuard _soundGuard;

        protected abstract string SoundKey { get; }
        protected virtual TimeSpan Cooldown => TimeSpan.FromSeconds(5);

        protected CombatLogSoundHandler(
            IDiscordVoiceService voiceService,
            SoundPlaybackGuard soundGuard)
        {
            _voiceService = voiceService;
            _soundGuard = soundGuard;
        }

        public async Task HandleAsync(
            TEvent evt,
            CancellationToken cancellationToken = default)
        {
            if (!_voiceService.IsConnected)
            {
                Console.WriteLine(
                    "Ignoring event because bot is not connected to voice.");

                return;
            }

            if (!_soundGuard.CanPlaySliding(SoundKey, Cooldown))
                return;

            await _voiceService.PlaySoundAsync(SoundKey, cancellationToken);
        }
    }
}
