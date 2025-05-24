using Dungeon.Game.Core.Level.Generator;
using Dungeon.Game.Core.Level.Utils;
using SharpGDX.Mathematics;

namespace Dungeon.Game.Core.Level.Elements.Tiles;

public class FloorTile : Tile
{
    public FloorTile(string texture, Vector2 position, DesignLabel designLabel) 
        : base(texture, position, designLabel)
    {
        LevelElement = LevelElement.Floor;
    }
}