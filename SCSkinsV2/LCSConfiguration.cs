using System;

namespace SCSkinsLib
{
    public class LCSConfiguration
    {

        public string PatcherConfigPath { get; set; }

        public string ToolsPath { get; set; }

        public string LolPath { get; set; }

        public string ModsPath { get; set; }

        public string ProfilesPath { get; set; }

        public bool NoTFT { get; set; }

        public bool IgnoreConflict { get; set; }


        public LCSConfiguration(string toolsPath, string lolPath, string patcherConfigPath, string modsPath, string profilesPath, bool noTft, bool ignoreConflict)
        {
            ToolsPath = toolsPath;
            LolPath = lolPath;
            PatcherConfigPath = patcherConfigPath;
            ModsPath = modsPath;
            ProfilesPath = profilesPath;
            NoTFT = noTft;
            IgnoreConflict = ignoreConflict;
        }

    }
}
