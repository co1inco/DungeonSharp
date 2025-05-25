using System.Collections.Specialized;
using CSharpFunctionalExtensions;
using Dungeon.Game.Core.Components;
using Dungeon.Game.Core.Level;
using Dungeon.Game.Core.Level.Elements;
using Dungeon.Game.Core.Level.Utils;
using Dungeon.Game.Core.Utils;
using Dungeon.Game.Helper;
using Serilog;
using SharpGDX.Scenes.Scene2D;
using Action = System.Action;

namespace Dungeon.Game.Core;

public sealed class ECSManagement
{
    private static readonly ILogger Log = Serilog.Log.ForContext<ECSManagement>();
    // TODO: should be ordered to execute systems in order of adding them 
    private static readonly Dictionary<Type, System> _systems = new(); // (SystemType, System)
    private static readonly List<System> _orderedSystems = new();
    private static readonly Dictionary<ILevel, ISet<EntitySystemMapper>> _levelStorageMap;
    private static ISet<EntitySystemMapper> _activeEntityStorage = new HashSet<EntitySystemMapper>();

    static ECSManagement()
    {
        _levelStorageMap = new Dictionary<ILevel, ISet<EntitySystemMapper>>
        {
            { new NullLevel(), _activeEntityStorage }
        };
        _activeEntityStorage.Add(new EntitySystemMapper());
    }

    public static void InformAboutChanges(Entity entity)
    {
        // throw new NotImplementedException();
    }


    #region Entities

    public static void Add(Entity entity)
    {
        _activeEntityStorage.ForEach(x => x.Add(entity));
        // _activeEntityStorage.ForEach((x) -> x.Add(entity));
        Log.Information("Entity: {Entity} will be added to the Game.", entity);
    }

    public static void Remove(Entity entity)
    {
        _activeEntityStorage.ForEach(x => x.Remove(entity));
        Log.Information("Entity: {Entity} will be removed from the Game.", entity);
    }

    public static IEnumerable<Entity> Entities() =>
        Entities([]);

    public static IEnumerable<Entity> Entities(System system) =>
        Entities(system.FilterRules);

    public static IEnumerable<Entity> Entities(IEnumerable<ComponentType> withComponents)
    {
        if (_activeEntityStorage
            .Where(x => x.FilterEquals(withComponents))
            .TryFirst()
            .TryGetValue(out var rf))
        {
            return rf.Entities();
        }
        else
        {
            return CreateEntitySystemMapper(withComponents).Entities();
        }
    }
    
    public static void RemoveAllEntities()
    {
        AllEntities().ForEach(Remove);
        Log.Information("All entities have ben removed from the game.");
    }

    public static IEnumerable<Entity> AllEntities()
    {
        return _levelStorageMap.Values
            .SelectMany(x => x
                .SelectMany(y => y
                    .Entities()));
    }
    
    public static Maybe<Entity> Find(IComponent component) => 
        AllEntities().TryFirst(x => x.Components.Contains(component));
    
    #endregion

    #region Systems
    
    public static IEnumerable<System> Systems => 
        _orderedSystems;

    public static Maybe<T> GetSystem<T>() where T : System =>
        _systems.TryGetValue(typeof(T), out var system) 
            ? (T)system 
            : Maybe<T>.None;
    
        
    public static void System<T>(Action<T> consumer) where T : System
    {
        if (_systems.TryGetValue(typeof(T), out var system))
        {
            consumer((T)system);
        }
    }
    
    public static Maybe<System> Add(System system)
    {
        _systems.TryGetValue(system.GetType(), out var currentSystem);
        _systems[system.GetType()] = system;

        var oldIndex = _orderedSystems.IndexOf(system);
        if (oldIndex >= 0)
            _orderedSystems[oldIndex] = system;
        else 
            _orderedSystems.Add(system);
        
        _activeEntityStorage
            .Where(x => x.Equals(system.FilterRules))
            .TryFirst()
            .Match(
                x => x.Add(system), 
                () => CreateEntitySystemMapper(system.FilterRules).Add(system));
        Log.Information("A new {System} was added to the game", system.GetType().Name);
        return currentSystem as System;
    }
    
    public static void Remove<T>() where T : System => 
        RemoveSystem(typeof(T));
    
    private static void RemoveSystem(Type systemType)
    {
        if (_systems.Remove(systemType, out var system))
        {
            _orderedSystems.Remove(system);
            _activeEntityStorage.ForEach(x => x.Remove(system));
        }
    }
    
    public static void RemoveAllSystems()
    {
        _systems.Keys
            .ToList() // Can modify the original collection while iterating it
            .ForEach(RemoveSystem);
    } 
    
    #endregion

    // ReadOnly dictionary?
    public static Dictionary<ILevel, ISet<EntitySystemMapper>> LevelStorageMap =>
        _levelStorageMap;

    public static void ActiveEntityStorage(ISet<EntitySystemMapper> entityStorage)
    {
        _activeEntityStorage = entityStorage;
    }
    
    public static void ActiveEntityStorage(IEnumerable<EntitySystemMapper> entityStorage)
    {
        _activeEntityStorage = new HashSet<EntitySystemMapper>(entityStorage);
    }
   

    public static Maybe<Entity> Hero() => Entities()
        .Where(x => x.IsPresent<PlayerComponent>())
        .TryFirst();

    
    
    private static EntitySystemMapper CreateEntitySystemMapper(IEnumerable<ComponentType> filter)
    {
        var mapper = new EntitySystemMapper(filter);
        _activeEntityStorage.Add(mapper);
        Entities().ForEach(x => mapper.Add(x));
        return mapper;
    }
}

public record struct NullLevel() : ILevel
{
    
    public void OnFirstLoad(Action callback) { }

    public void OnLoad() { }

    public int GetNodeCount() => 0;

    public void AddTile(Tile tile) { }

    public void RemoveTile(Tile tile) { }

    public IEnumerable<Tile> Tiles => [];

    Tile ILevel.StartTile { get; set; }

    public IEnumerable<Tile> EndTiles() => [];

    public Tile[][] Layout => [];

    public Maybe<Tile> TileAt(Coordinate coordinate) => Maybe<Tile>.None;

    public void AddFloorTile() { }

    public void AddWallTile() { }

    public void AddHoleTile() { }
}