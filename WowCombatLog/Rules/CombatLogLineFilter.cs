using FileReader.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace WowCombatLog.Rules
{
    public class CombatLogLineFilter : ILineFilter
    {
        private readonly IEnumerable<ICombatLogLineRule> _rules;

        public CombatLogLineFilter(IEnumerable<ICombatLogLineRule> rules)
        {
            _rules = rules;
        }

        public bool ShouldPublish(string line)
        {
            return _rules.Any(rule => rule.Matches(line));
        }
    }
}
