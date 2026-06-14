using LocalDiscordApp.Service;
using LocalDiscordApp.Sounds;
using System;
using System.Collections.Generic;
using System.Text;
using WowCombatLog.Events;

namespace LocalDiscordApp.CombatLog
{
    public class CombatLogResurrectHandler
    : CombatLogSoundHandler<WowResurrectEvent>
    {
        protected override string SoundKey => "resurrect";

        public CombatLogResurrectHandler(
            IDiscordVoiceService voiceService,
            SoundPlaybackGuard soundGuard)
            : base(voiceService, soundGuard)
        {
        }
    }
}
