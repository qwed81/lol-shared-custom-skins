using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SCSkinsLib
{
    public class LCSTools
    {

        public LCSConfiguration Configuration { get; set; }

        public LCSTools(LCSConfiguration config)
        {
            Configuration = config;
        }

        private string noTFTString => Configuration.NoTFT ? "--noTFT" : "";

        private string ignoreConflictsString => Configuration.IgnoreConflict ? "--ignoreConflit" : "";

        private string gameString => $"--game:\"{Configuration.LolPath}\"";

        public async Task<bool> InstallModAsync(IProgress<string> progress, string sourcePath, string? name = null)
        {
            string modName;
            if (name == null)
                modName = Path.GetFileNameWithoutExtension(sourcePath);
            else
                modName = name;

            string parameters = $"import \"{sourcePath}\" \"{Path.Combine(Configuration.ModsPath, modName)}\" " +
                                $"{gameString} {noTFTString}";

            return await ProcessRun(progress, parameters);
        }

        public async Task<bool> CreateOverlayAsync(IProgress<string> progress, string profileName, List<string> modNames)
        {
            string mods = "--mods:\"";
            for(int i = 0; i < modNames.Count; i++)
            {
                mods += modNames[i];
                if (i != modNames.Count - 1)
                    mods += "/";
            }
            mods += "\"";

            string parameters = $"mkoverlay \"{Configuration.ModsPath}\" \"{Path.Combine(Configuration.ProfilesPath, profileName)}\" " +
                                $"{gameString} {mods} {noTFTString} {ignoreConflictsString}";

            return await ProcessRun(progress, parameters);
        }

        public async Task<bool> RunOverlayAsync(IProgress<string> progress, string profileName, CancellationToken token)
        {
            string parameters = $"runoverlay \"{Path.Combine(Configuration.ProfilesPath, profileName)}\" {Configuration.PatcherConfigPath}" +
                $"{gameString}";

            return await ProcessRun(progress, parameters, token);
        }

        public async Task<bool> ProcessRun(IProgress<string> progress, string parameters, CancellationToken? token = null)
        {
            bool completed = await Task.Run(async () => {

                ProcessStartInfo startInfo = new ProcessStartInfo()
                {
                    FileName = Configuration.ToolsPath,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    Arguments = parameters
                };

                Process process = Process.Start(startInfo);

                if (token != null)
                {
                    token.Value.Register(() => {
                        process.StandardInput.Write("\n"); // cancels the operation
                    });
                }

                while (process.HasExited == false)
                {
                    string output = await process.StandardOutput.ReadLineAsync();
                    progress.Report(output);
                }

                return process.ExitCode == 0;
            
            });

            return completed;
        }
        

    }
}
