using CSharpFunctionalExtensions;
using Dungeon.Game.Core.Components;
using Dungeon.Game.Core.Level.Utils;
using Vector2 = SharpGDX.Mathematics.Vector2;

namespace Dungeon.Game.Core.Level.Elements;

public interface ILevel
{

    void OnFirstLoad(Action callback);

    void OnLoad();

    int GetNodeCount();
    
    void AddTile(Tile tile);

    void RemoveTile(Tile tile);

    // IEnumerable<Tile> Tiles { get; } 

    Tile StartTile { get; set; }

    IEnumerable<Tile> EndTiles();
    
    
    Tile[][] Layout { get; }
    
    
    
    
    void AddFloorTile();

    void AddWallTile();

    void AddHoleTile();

}

public static class LevelExtensions
{
    public static (int, int) Size(this ILevel level) => 
        (level.Layout[0].Length, level.Layout.Length);

    public static Maybe<Tile> TileAt(this ILevel level, Coordinate coordinate)
    {
        if (level.Layout.Length < coordinate.Y && coordinate.Y >= 0)
        {
            if (level.Layout[coordinate.Y].Length < coordinate.X && coordinate.X >= 0)
            {
                return level.Layout[coordinate.Y][coordinate.X];
            } 
        }
        return Maybe<Tile>.None;
    }

    public static Maybe<Tile> TileAt(this ILevel level, Coordinate coordinate, PositionComponent.Direction direction) =>
        level.TileAt(coordinate + direction switch
        {
            PositionComponent.Direction.Up => new Coordinate(0, 1),
            PositionComponent.Direction.Down => new Coordinate(0, -1),
            PositionComponent.Direction.Left => new Coordinate(-1, 0),
            PositionComponent.Direction.Right => new Coordinate(1, 0),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        });

    public static (Maybe<Tile> Up, Maybe<Tile> Down, Maybe<Tile> Left, Maybe<Tile> Right)
        Neighbours(this ILevel level, Coordinate coordinate) =>
    (
        level.TileAt(coordinate, PositionComponent.Direction.Up),
        level.TileAt(coordinate, PositionComponent.Direction.Down),
        level.TileAt(coordinate, PositionComponent.Direction.Left),
        level.TileAt(coordinate, PositionComponent.Direction.Right)
    );

    public static IEnumerable<Tile> NeighboursList(this ILevel level, Coordinate coordinate) =>
        Enum.GetValues<PositionComponent.Direction>()
            .Select(x => level.TileAt(coordinate, x))
            .Choose();

    public static Tile RandomTile(this ILevel level)
    {
        var row = level.Layout[Random.Shared.Next(0, level.Layout.Length)];
        return row[Random.Shared.Next(0, row.Length)];
    }

    public static Maybe<Tile> EndTile(this ILevel level) => 
        level.EndTiles().TryFirst();

    public static IEnumerable<Tile> Tiles(this ILevel level) =>
        level.Layout.SelectMany(x => x);

    public static Vector2 PositionOf(this ILevel level, Entity entity) => // why is this part of level???
        entity.Fetch<PositionComponent>()
            .GetValueOrThrow(new Exception("Missing position component"))
            .Position;
}