
using Dungeon.Game.Helper;
using Serilog;

namespace Dungeon.Game.Core;

public class Consumer<T> {}

public abstract class System
{
    public const int DEFAULT_EVERY_FRAME_EXECUTE = 1;
    
    protected static readonly ILogger Log = Serilog.Log.ForContext<System>();
    private readonly ComponentType[] _filterRules;
    protected bool _run = true;

    
    public System(int executeEveryXFrames, params ComponentType[] filterRules)
    {
        //TODO: C#13 filterRules should be a set
        // filterRules.ValidateFilterRules();
        _filterRules = filterRules.ToArray();
        ExecuteEveryXFrames = executeEveryXFrames;
        Log.Information("A new {System} was created", GetType().Name);
    }

    public System(params ComponentType[] filterRules) : this(DEFAULT_EVERY_FRAME_EXECUTE, filterRules)
    {
        
    }

    
    protected Action<Entity> OnEntityAdd { get; set; } = _ => { };
    
    protected Action<Entity> OnEntityRemove { get; set; } = _ => { };
    
    public bool IsRunning => _run;
    
    public int ExecuteEveryXFrames { get; }

    public int LastExecutedInFrame { get; set; }
    
    public IReadOnlyCollection<ComponentType> FilterRules => _filterRules;

    
    public abstract void Execute();
    
    public void TriggerOnAdd(Entity entity)  =>
        OnEntityAdd?.Invoke(entity);
    
    public void TriggerOnRemove(Entity entity)  =>
        OnEntityRemove?.Invoke(entity);


    public virtual void Run()
    {
        if (!_run)
            Log.Information("{System} is now running", GetType().Name);
        _run = true;
    }

    public virtual void Stop()
    {
        if (_run)
            Log.Information("{System} is now paused", GetType().Name);
        _run = false;
    }

    public IEnumerable<Entity> FilteredEntities(IEnumerable<ComponentType> requiredComponents)
    {
        // requiredComponents.ValidateFilterRules();
        return DungeonGame.Entities(requiredComponents);
    }

    public IEnumerable<Entity> FilteredEntities()
    {
        return DungeonGame.Entities(_filterRules);
    }
    
    public IEnumerable<Entity> FilteredEntities(params ComponentType[] requireComponents)
    {
        // requireComponents.ValidateFilterRules();
        return DungeonGame.Entities(requireComponents);
    }
    
    public IEnumerable<Entity> FilteredEntities<T>() 
        where T: IComponent => 
        FilteredEntities(ComponentType.Is<T>());
    
    public IEnumerable<Entity> FilteredEntities<T1, T2>() 
        where T1: IComponent  
        where T2: IComponent => 
        FilteredEntities(ComponentType.Is<T1>(), ComponentType.Is<T2>());
    
    public IEnumerable<Entity> FilteredEntities<T1, T2, T3>()
        where T1: IComponent  
        where T2: IComponent 
        where T3: IComponent => 
        FilteredEntities(ComponentType.Is<T1>(), ComponentType.Is<T2>(), ComponentType.Is<T3>());
    
    public IEnumerable<Entity> FilteredEntities<T1, T2, T3, T4>() 
        where T1: IComponent  
        where T2: IComponent 
        where T3: IComponent  
        where T4: IComponent => 
        FilteredEntities(ComponentType.Is<T1>(), ComponentType.Is<T2>(), ComponentType.Is<T3>(), ComponentType.Is<T4>());
    
    public IEnumerable<EntityComponentWrapper<T>> FilteredEntitiesF<T>() 
        where T: IComponent => 
        FilteredEntities<T>().Select(x => new EntityComponentWrapper<T>(x));
    
    public IEnumerable<EntityComponentWrapper<T1, T2>> FilteredEntitiesF<T1, T2>() 
        where T1: IComponent  
        where T2: IComponent => 
        FilteredEntities<T1, T2>().Select(x => new EntityComponentWrapper<T1, T2>(x));
    
    public IEnumerable<EntityComponentWrapper<T1, T2, T3>> FilteredEntitiesF<T1, T2, T3>() 
        where T1: IComponent  
        where T2: IComponent  
        where T3: IComponent => 
        FilteredEntities<T1, T2, T3>().Select(x => new EntityComponentWrapper<T1, T2, T3>(x));
}

public static class SystemExtensions
{
    // public static IEnumerable<EntityComponentWrapper<T>> FilteredEntitiesF<T>(this System system) 
    //     where T: IComponent => 
    //     system.FilteredEntities<T>().Select(x => new EntityComponentWrapper<T>(x));
    //
    // public static IEnumerable<EntityComponentWrapper<T1, T2>> FilteredEntities<T1, T2>(this System system) 
    //     where T1: IComponent  
    //     where T2: IComponent => 
    //     system.FilteredEntities<T1, T2>().Select(x => new EntityComponentWrapper<T1, T2>(x));
    
    // public static IEnumerable<Entity> FilteredEntities<T1, T2, T3>(this System system)
    //     where T1: IComponent  
    //     where T2: IComponent 
    //     where T3: IComponent => 
    //     system.FilteredEntities(ComponentType.Is<T1>(), ComponentType.Is<T2>(), ComponentType.Is<T3>());
    //
    // public static IEnumerable<Entity> FilteredEntities<T1, T2, T3, T4>(this System system) 
    //     where T1: IComponent  
    //     where T2: IComponent 
    //     where T3: IComponent  
    //     where T4: IComponent => 
    //     system.FilteredEntities(ComponentType.Is<T1>(), ComponentType.Is<T2>(), ComponentType.Is<T3>(), ComponentType.Is<T4>());
}

public class EntityComponentWrapper<T1>(Entity entity) where T1 : IComponent
{
    public Entity Entity => entity;

    private T1? _component; // Lazy evaluation
    public T1 Component => _component ??= entity.FetchOrThrow<T1>();
}

public class EntityComponentWrapper<T1, T2>(Entity entity) where T1 : IComponent where T2 : IComponent
{
    public Entity Entity => entity;

    private T1? _component1; // Lazy evaluation
    public T1 Component1 => _component1 ??= entity.FetchOrThrow<T1>();

    private T2? _component2; // Lazy evaluation
    public T2 Component2 => _component2 ??= entity.FetchOrThrow<T2>();
}

public class EntityComponentWrapper<T1, T2, T3>(Entity entity) where T1 : IComponent where T2 : IComponent where T3 : IComponent
{
    public Entity Entity => entity;

    private T1? _component1; // Lazy evaluation
    public T1 Component1 => _component1 ??= entity.FetchOrThrow<T1>();

    private T2? _component2; // Lazy evaluation
    public T2 Component2 => _component2 ??= entity.FetchOrThrow<T2>();
    
    private T3? _component3; // Lazy evaluation
    public T3 Component3 => _component3 ??= entity.FetchOrThrow<T3>();
}