using System;
using System.Collections.Generic;
using System.Text;

namespace WowCombatLog.Rules
{
    public class CombatResurrectionLineRule : ICombatLogLineRule
    {
        public bool Matches(string line)
        {
            return line.Contains("SPELL_RESURRECT")
                && (
                    line.Contains("Soulstone Resurrection")
                    || line.Contains("Rebirth")
                );
        }
    }
}
