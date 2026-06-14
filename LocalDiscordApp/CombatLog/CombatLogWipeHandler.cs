using LocalDiscordApp.Service;
using LocalDiscordApp.Sounds;
using System;
using System.Collections.Generic;
using System.Text;
using WowCombatLog.Events;

namespace LocalDiscordApp.CombatLog
{
    public class CombatLogWipeHandler
    : CombatLogSoundHandler<WowWipeEvent>
    {
        protected override string SoundKey => "wipe";

        protected override TimeSpan Cooldown => TimeSpan.FromSeconds(30);

        public CombatLogWipeHandler(
            IDiscordVoiceService voiceService,
            SoundPlaybackGuard soundGuard)
            : base(voiceService, soundGuard)
        {
        }
    }
}
