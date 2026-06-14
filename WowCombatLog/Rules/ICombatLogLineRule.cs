using System;
using System.Collections.Generic;
using System.Text;

namespace WowCombatLog.Rules
{
    public interface ICombatLogLineRule
    {
        bool Matches(string line);
    }
}
