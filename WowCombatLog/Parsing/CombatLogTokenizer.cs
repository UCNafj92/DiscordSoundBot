using System;
using System.Collections.Generic;
using System.Text;

namespace WowCombatLog.Parsing
{
    public static class CombatLogTokenizer
    {
        public static List<string> Tokenize(string line)
        {
            var fields = new List<string>();
            var current = new StringBuilder();
            var insideQuotes = false;

            foreach (var character in line)
            {
                if (character == '"')
                {
                    insideQuotes = !insideQuotes;
                    continue;
                }

                if (character == ',' && !insideQuotes)
                {
                    fields.Add(current.ToString());
                    current.Clear();
                    continue;
                }

                current.Append(character);
            }

            fields.Add(current.ToString());

            return fields;
        }
    }
}
