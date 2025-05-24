using SharpGDX.Mathematics;

namespace Dungeon.Game.Core.Components;

public sealed class VelocityComponent : IComponent
{
    private Vector2 _currentVelocity = Vector2.Zero;
    private Vector2 _previousVelocity = Vector2.Zero;


    public VelocityComponent() { }
    
    
    public Vector2 Velocity { get; set; } = Vector2.Zero;

    public Action<Entity> OnWallHitCallback { get; set; } = _ => { };

    public bool CanEnterOpenPits { get; set; } = false;

}