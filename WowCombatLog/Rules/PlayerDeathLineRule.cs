using System;
using System.Collections.Generic;
using System.Text;

namespace WowCombatLog.Rules
{
    public class PlayerDeathLineRule : ICombatLogLineRule
    {
        public bool Matches(string line)
        {
            return line.Contains("UNIT_DIED")
                && line.Contains("Player-");
        }
    }
}
