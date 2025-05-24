using SharpGDX.Mathematics;

namespace Dungeon.Game.Core.Components;

public sealed class PositionComponent : IComponent
{
    public static readonly Vector2 IllegalPosition = new (int.MinValue, int.MinValue);
    
    public enum Direction
    {
        Up, Down, Left, Right
    }


    public Vector2 Position { get; set; } = IllegalPosition;

    public Direction ViewDirection { get; set; } = Direction.Down;
}

public static class PositionDirectionExtensions
{
    public static PositionComponent.Direction Opposite(this PositionComponent.Direction direction) => direction switch
    {
        PositionComponent.Direction.Up => PositionComponent.Direction.Down,
        PositionComponent.Direction.Down => PositionComponent.Direction.Up,
        PositionComponent.Direction.Left => PositionComponent.Direction.Right,
        PositionComponent.Direction.Right => PositionComponent.Direction.Left,
        _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
    };
}