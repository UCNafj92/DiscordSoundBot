using EventBus.Abstractions;
using LocalDiscordApp.Service;
using LocalDiscordApp.Sounds;
using System;
using System.Collections.Generic;
using System.Text;
using WowCombatLog.Events;

namespace LocalDiscordApp.CombatLog
{
    public class CombatLogDeathHandler
    : CombatLogSoundHandler<WowDeathEvent>
    {
        protected override string SoundKey => "death";

        public CombatLogDeathHandler(
            IDiscordVoiceService voiceService,
            SoundPlaybackGuard soundGuard)
            : base(voiceService, soundGuard)
        {
        }
    }
}
