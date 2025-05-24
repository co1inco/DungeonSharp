using Serilog;

namespace Dungeon.Game.Core;

public class PreRunConfiguration
{
    public static int WindowWidth { get; set; } =  1280;
    public static int WindowHeight { get; set; } =  720;
    public static int FrameRate { get; set; } =  30;
    public static bool FullScreen { get; set; } =  false;
    
    public static bool Resizable { get; set; } =  false;
    public static string WindowTitle { get; set; } = "PM-Dungeon";
    public static string LogoPath { get; set; } = "Assets/logo/cat_logo_35x35.png";

    public static bool DisableAudio { get; set; } = false;
    
    public static Action? UserOnFrame { get; set; }
    public static Action? UserOnSetup { get; set; }
    public static Action<bool>? UserOnLevelLoad { get; set; }

    public static void LoadConfig()
    {
        Log.Error("Loading configuration is not implemented.");
    }
}