using Serilog;

namespace Dungeon.Game.Core;

public sealed class DungeonGame
{
    private static readonly ILogger Log = Serilog.Log.ForContext<DungeonGame>();
                               
    public static void Run()
    {
        // GameLoop.Run()
    }

    public static void Add(Entity entity)
    {
        ECSManagement.Add(entity);
    }

    public static void Remove(Entity entity)
    {
        ECSManagement.Remove(entity);
    }

    public static IEnumerable<Entity> Entities() => 
        ECSManagement.Entities();
    
    public static IEnumerable<Entity> Entities(System system) => 
        ECSManagement.Entities(system);
    
    public static IEnumerable<Entity> Entities(IEnumerable<ComponentType> requireComponents) => 
        ECSManagement.Entities(requireComponents);
}