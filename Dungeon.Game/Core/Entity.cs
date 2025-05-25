using CSharpFunctionalExtensions;
using Dungeon.Game.Core.Utils.Components;
using Serilog;

namespace Dungeon.Game.Core;

public sealed class Entity : IComparable<Entity>, IEquatable<Entity>
{
    private static readonly ILogger Log = Serilog.Log.ForContext<Entity>();
    private static int _nextId = 0;

    private readonly Dictionary<ComponentType, IComponent> _components = [];


    public Entity(string name)
    {
        Name = name;
        Id = _nextId++;
        Log.Information("Created entity {Id}", Id);
    }

    public Entity() : this($"_{_nextId}")
    {
        
    }
    
    public int Id { get; }

    public string Name { get; }


    public void Add(IComponent component)
    {
        _components[component.GetComponentType()] = component;
        ECSManagement.InformAboutChanges(this);
        Log.Information("Added component {Component} to {Id}", component.GetType().Name, Id);
    }

    public void Remove<T>() where T : IComponent
    {
        if (_components.Remove(ComponentType.Is<T>()))
        {
            ECSManagement.InformAboutChanges(this);
            Log.Information("Removed component {Component} from {Id}", typeof(T).Name, Id);
        }
    }

    public void Remove(ComponentType componentType)
    {
        if (_components.Remove(componentType))
        {
            // ECSManagement.InformAboutChanges(this);
            Log.Information("Removed component {Component} from {Id}", componentType.Type.Name, Id);
        }
    }
    
    public Maybe<T> Fetch<T>() where T : IComponent
    {
        if (_components.TryGetValue(ComponentType.Is<T>(), out var component))
            return (T)component;
        return Maybe<T>.None;
    }

    public Maybe<object> Fetch(ComponentType componentType)
    {
        return _components.TryGetValue(componentType, out var component) 
            ? Maybe<object>.From(component) 
            : Maybe<object>.None;
    }
    
    public bool IsPresent<T>() where T : IComponent => 
        _components.ContainsKey(ComponentType.Is<T>());
    
    public bool IsPresent(ComponentType componentType) => 
        _components.ContainsKey(componentType);
    
    public IEnumerable<IComponent> Components =>
        _components.Values;
    
    
    public override string ToString()
    {
        var idString = $"_{Id}";
        return Name.Contains(idString) ? Name : $"{Name}{idString}";
    }

    
    #region Comparable
    
    public int CompareTo(Entity? other) => 
        Id - (other?.Id ?? 0);

    public static bool operator <(Entity a, Entity b)
    {
        return a.CompareTo(b) < 0;
    }

    public static bool operator >(Entity a, Entity b)
    {
        return a.CompareTo(b) > 0;
    }

    public static bool operator <=(Entity a, Entity b)
    {
        return a.CompareTo(b) <= 0;
    }

    public static bool operator >=(Entity a, Entity b)
    {
        return a.CompareTo(b) >= 0;
    }
    
    #endregion
    

    #region Equality

    public bool Equals(Entity? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return _components.Equals(other._components) && Id == other.Id;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is Entity other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_components, Id, Name);
    }
    
    #endregion

}

public static class EntityExtensions
{
    public static T FetchOrThrow<T>(this Entity entity) where T : IComponent =>
        entity.Fetch<T>().GetValueOrThrow(MissingComponentException.Create<T>(entity));
}