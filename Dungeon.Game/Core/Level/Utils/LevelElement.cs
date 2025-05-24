namespace Dungeon.Game.Core.Level.Utils;

public enum LevelElement
{
    Skip,
    Floor,
    Wall,
    Hole,
    Exit,
    Pit,
    Door
}

public static class LevelElementExtensions
{
    public static bool IsAccessible(this LevelElement levelElement) => levelElement switch
    {
        LevelElement.Skip => false,
        LevelElement.Floor => true,
        LevelElement.Wall => false,
        LevelElement.Hole => false,
        LevelElement.Exit => true,
        LevelElement.Pit => false,
        LevelElement.Door => true,
        _ => throw new ArgumentOutOfRangeException(nameof(levelElement), levelElement, null)
    };

    public static bool CanSeeThrough(this LevelElement levelElement) => levelElement switch
    {
        LevelElement.Skip => false,
        LevelElement.Floor => true,
        LevelElement.Wall => false,
        LevelElement.Hole => false,
        LevelElement.Exit => false,
        LevelElement.Pit => true,
        LevelElement.Door => true,
        _ => throw new ArgumentOutOfRangeException(nameof(levelElement), levelElement, null)
    };
}