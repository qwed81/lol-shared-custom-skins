using SCSkinsLib;

LCSConfiguration config = new LCSConfiguration(
    toolsPath: @"C:\Users\josha\Downloads\cslol-manager-windows\cslol-manager\cslol-tools\mod-tools.exe",
    lolPath: @"C:\Riot Games\League of Legends\Game",
    patcherConfigPath: @"C:\Users\josha\Downloads\cslol-manager-windows\cslol-manager\cslol-tools\hashes.game.txt",
    modsPath: @"C:\Users\josha\Desktop\mods",
    profilesPath: @"C:\Users\josha\Desktop\profiles",
    noTft: true,
    ignoreConflict: true
);

string modSourcePath = @"C:\Users\josha\Downloads\Nepgear The Purple Sister V2 by MabuLaboo.fantome";

LCSTools tools = new LCSTools(config);
bool success = await tools.InstallModAsync(new ConsoleLogProgress(), modSourcePath);
Console.WriteLine("success: " + success);

var mods = new List<string>() { "mod" };
success = await tools.CreateOverlayAsync(new ConsoleLogProgress(), "profile 1", mods);
Console.WriteLine("success:" + success);

CancellationTokenSource cts = new CancellationTokenSource();
/*
_ = Task.Run(async () =>
{
    await Task.Delay(10000);
    cts.Cancel();
    Console.WriteLine("canceled");
});
*/

success = await tools.RunOverlayAsync(new ConsoleLogProgress(), "profile 1", cts.Token);
Console.WriteLine("success:" + success);

class ConsoleLogProgress : IProgress<string>
{
    public void Report(string value)
    {
        Console.WriteLine(value);
    }
}