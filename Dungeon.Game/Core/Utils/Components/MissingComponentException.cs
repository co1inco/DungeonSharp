namespace Dungeon.Game.Core.Utils.Components;

public class MissingComponentException : Exception
{
    public Entity Entity { get; }
    public Type ComponentType { get; }

    public MissingComponentException(Entity entity, Type componentType) : base($"Missing component: {componentType.Name}")
    {
        Entity = entity;
        ComponentType = componentType;
    }
    
    public static MissingComponentException Create<T>(Entity entity) where T : IComponent =>
        new MissingComponentException(entity, typeof(T));
}