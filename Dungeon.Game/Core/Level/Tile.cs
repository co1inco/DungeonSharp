using SharpGDX.Mathematics;

namespace Dungeon.Game.Core.Level;

public abstract class Tile
{
    private readonly Vector2 _globalPosition;
    private const float DEFAULT_FRICTION = 0.8f;


    public Tile(Vector2 globalPosition)
    {
        _globalPosition = globalPosition;
    }

    public Vector2 Position => _globalPosition;

}