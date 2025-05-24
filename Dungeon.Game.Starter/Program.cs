// See https://aka.ms/new-console-template for more information

using Dungeon.Game.Core;
using Serilog;
using SharpGDX;

Console.WriteLine("Hello, World!");

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .MinimumLevel.Information()
    .CreateLogger();
    
// DungeonGame.LoadConfig()
// DungeonGame.DisableAudio()
// DungeonGame.FrameRate = 30;
DungeonGame.Run();