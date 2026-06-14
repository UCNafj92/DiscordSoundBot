using Discord.Audio;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocalDiscordApp.Service
{
    public interface IDiscordVoiceService
    {
        Task JoinAsync(ulong guildId, ulong voiceChannelId);

        Task PlaySoundAsync(
            string soundKey,
            CancellationToken cancellationToken = default);

        Task DisconnectAsync();

        public bool IsConnected { get; }
    }
}
