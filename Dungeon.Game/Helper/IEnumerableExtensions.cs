namespace Dungeon.Game.Helper;

public static class EnumerableExtensions
{

    public static void ForEach<T>(this IEnumerable<T> collection)
    {
        foreach (var _ in collection) { }
    }
    
    public static void ForEach<T>(this IEnumerable<T> collection, Action<T> predicate)
    {
        foreach (var x in collection)
        {
            predicate(x);
        }
    }
}