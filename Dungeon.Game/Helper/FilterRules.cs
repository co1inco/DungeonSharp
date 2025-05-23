using Dungeon.Game.Core;

namespace Dungeon.Game.Helper;

public static class FilterRules
{
    public static void ValidateFilterRules(this IEnumerable<Type> filterRules)
    {
#if DEBUG
        // Throw in debug mode to notify developer, but does not actually break anything
        if (filterRules.Any(x => !x.IsAssignableTo(typeof(IComponent))))
            throw new ArgumentException("Filtering rules must implement IComponent");
#endif
    }
}