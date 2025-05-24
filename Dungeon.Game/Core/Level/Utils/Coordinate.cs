using OpenTK.Mathematics;

namespace Dungeon.Game.Core.Level.Utils;

public record Coordinate(int X, int Y)
{

    public Vector2 ToPoint() => new Vector2(X, Y);
    public Vector2 ToCenterPoint() => new Vector2(X + 0.5f, Y + 0.5f);
    
    public static Coordinate operator+(Coordinate a, Coordinate b) => 
        new Coordinate(a.X + b.X, a.Y + b.Y);
    
    public static Coordinate operator-(Coordinate a, Coordinate b) => 
        new Coordinate(a.X + b.X, a.Y + b.Y);

    public float Distance(Coordinate other) => Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
    
    public override string ToString() => $"Coordinator(X={X}, y={Y})";
}

public static class CoordinateExtensions
{
    public static Coordinate ToCoordinate(this Vector2 point) => 
        new Coordinate((int)point.X, (int)point.Y);
}