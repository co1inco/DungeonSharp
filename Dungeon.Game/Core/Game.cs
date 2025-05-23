using Serilog;

namespace Dungeon.Game.Core;

public sealed class Game
{
    private static readonly ILogger Log = Serilog.Log.ForContext<Game>();
                                                                                
    public Game()
    {
        
    }

    public static void Run()
    {
        // GameLoop.Run()
    } 
    
    
}