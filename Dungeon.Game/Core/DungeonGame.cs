using CSharpFunctionalExtensions;
using Dungeon.Game.Core.Level;
using Dungeon.Game.Core.Level.Elements;
using Dungeon.Game.Core.Systems;
using Serilog;

namespace Dungeon.Game.Core;

public sealed class DungeonGame
{
    private static readonly ILogger Log = Serilog.Log.ForContext<DungeonGame>();
                               
    public static void Run()
    {
        GameLoop.Run();
    }

    #region Entities
    
    public static void Add(Entity entity)
    {
        ECSManagement.Add(entity);
    }

    public static void Remove(Entity entity)
    {
        ECSManagement.Remove(entity);
    }
    
    public static void RemoveAllEntities() => 
        ECSManagement.RemoveAllEntities(); 

    public static IEnumerable<Entity> Entities() => 
        ECSManagement.Entities();
    
    public static IEnumerable<Entity> Entities(System system) => 
        ECSManagement.Entities(system);
    
    public static IEnumerable<Entity> Entities(IEnumerable<ComponentType> requireComponents) => 
        ECSManagement.Entities(requireComponents);

    public static Maybe<Entity> Find(IComponent component) => 
        ECSManagement.Find(component);
    
    #endregion

    #region Systems

    public static Maybe<System> Add(System system) => 
        ECSManagement.Add(system);

    public static IEnumerable<System> Systems() => 
        ECSManagement.Systems;
    
    public static Maybe<T> GetSystem<T>() where T : System => 
        ECSManagement.GetSystem<T>();
    
    public static void RemoveAllSystems() =>
        ECSManagement.RemoveAllSystems();
    
    #endregion

    #region Level

    public static Tile? StartTile() => 
        CurrentLevel?.StartTile;

    public static Tile? EndTile() => 
        CurrentLevel?.EndTile().Match(x => x, () => null!);

    public static ILevel? CurrentLevel
    {
        get => LevelSystem.CurrentLevel;
        set
        {
            ECSManagement.GetSystem<LevelSystem>()
                .Match(
                    s => s.LoadLevel(value), 
                    () => Log.Warning("Can not set level because no level system was found."));
        }
    }

    #endregion
    
    
    
    
    // public static Tile 
}