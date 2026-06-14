using System;
using System.Collections.Generic;
using System.Text;

namespace LocalDiscordApp.Sounds
{
    public class SoundOptions
    {
        public string FolderPath { get; set; } = string.Empty;

        public Dictionary<string, string> Mappings { get; set; }
            = new();
    }
}
