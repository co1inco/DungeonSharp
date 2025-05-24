using Dungeon.Game.Core.Level.Generator;
using Dungeon.Game.Core.Level.Utils;
using SharpGDX.Mathematics;

namespace Dungeon.Game.Core.Level;

public abstract class Tile
{
    private const float DEFAULT_FRICTION = 0.8f;


    protected Tile(string texture, Vector2 position, DesignLabel designLabel)
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

    public virtual string TexturePath { get; }
    
    public int TintColor { get; protected set; }
}