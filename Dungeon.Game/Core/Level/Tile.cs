using CSharpFunctionalExtensions;
using Dungeon.Game.Core.Level.Generator;
using Dungeon.Game.Core.Level.Utils;
using Dungeon.Game.Core.Utils.Components.Path;
using SharpGDX.Mathematics;

namespace Dungeon.Game.Core.Level;

public abstract class Tile
{
    public const float DEFAULT_FRICTION = 0.8f;


    protected Tile(IPath texture, Vector2 position, DesignLabel designLabel)
    {
        LevelElement = LevelElement.Skip;
        Position = position;
        DesignLabel = designLabel;
        TexturePath = texture;
    }

    public Vector2 Position { get; }
    public DesignLabel DesignLabel { get; }

    public LevelElement LevelElement { get; protected set; }

    public virtual bool Visible { get; protected set; } = true;

    public virtual IPath TexturePath { get; }
    
    public int TintColor { get; protected set; }
    
    public float Friction { get; set; } = DEFAULT_FRICTION;
}

public static class TileExtensions
{
    public static float GetFrictionOrDefault(this Maybe<Tile>? tile)
    {
        return tile?.Match(x => x.Friction, () => Tile.DEFAULT_FRICTION) ?? Tile.DEFAULT_FRICTION;
    }
}