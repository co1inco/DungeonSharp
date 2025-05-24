using Dungeon.Game.Core.Level.Generator;
using Dungeon.Game.Core.Level.Utils;
using SharpGDX.Mathematics;

namespace Dungeon.Game.Core.Level.Elements.Tiles;

public class PitTile : Tile
{
    public PitTile(string texture, Vector2 position, DesignLabel designLabel) 
        : base(texture, position, designLabel)
    {
        LevelElement = LevelElement.Pit;
        IsOpen = false;
        TimeToOpen = 0;
    }

    public bool IsOpen { get; set; }
    
    public int TimeToOpen { get; set; }
    
}