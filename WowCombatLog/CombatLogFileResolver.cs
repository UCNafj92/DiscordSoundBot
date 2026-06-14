using System;
using System.Collections.Generic;
using System.Text;

namespace WowCombatLog
{
    public class CombatLogFileResolver
    {
        public string GetLatestCombatLogPath(string folderPath)
        {
            var file = Directory
                .GetFiles(folderPath, "WoWCombatLog-*.txt")
                .Select(path => new FileInfo(path))
                .OrderByDescending(file => file.LastWriteTimeUtc)
                .FirstOrDefault();

            if (file is null)
                throw new FileNotFoundException(
                    $"No WoW combat log found in '{folderPath}'.");
            Console.WriteLine("🔹 Latest combat log found: " + file.FullName);
            return file.FullName;
        }
    }
}
