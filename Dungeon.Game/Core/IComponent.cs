using System.ComponentModel;
using SharpGDX.Scenes.Scene2D;

namespace Dungeon.Game.Core;

public interface IComponent
{
    
}

public readonly struct ComponentType : IEquatable<ComponentType>
{
    public ComponentType(Type type)
    {
        if (!type.IsAssignableTo(typeof(IComponent)))
            throw new ArgumentException($"Type {type} is not an IComponent");
        Type = type;
    }

    public static ComponentType Is<T>() => new ComponentType(typeof(T));
    
    // Not sure if this is good...
    public static implicit operator ComponentType(Type type) => new ComponentType(type);

    
    public Type Type { get; }

    
    public bool Equals(ComponentType other) => Type == other.Type;
    public override bool Equals(object? obj) => obj is ComponentType other && Equals(other);
    public override int GetHashCode() => Type.GetHashCode();
}

public static class ComponentTypeExtensions
{
    public static ComponentType GetComponentType(this IComponent component) => 
        new ComponentType(component.GetType());
}