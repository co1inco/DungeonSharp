using SharpGDX.Mathematics;

namespace Dungeon.Game.Core.Components;

public sealed class VelocityComponent : IComponent
{
    public static bool DeltaTimeAwareDefault { get; set; } = false;
    // private Vector2 _currentVelocity = Vector2.Zero;
    private Vector2 _previousVelocity = Vector2.Zero;


    public VelocityComponent() { }
    
    
    public Vector2 Velocity { get; set; } = Vector2.Zero;
    
    public Vector2 CurrentVelocity { get; set; } = Vector2.Zero;

    public Action<Entity> OnWallHitCallback { get; set; } = _ => { };

    public bool CanEnterOpenPits { get; set; } = false;

    public bool UseDeltaTime { get; set; } = DeltaTimeAwareDefault;

}