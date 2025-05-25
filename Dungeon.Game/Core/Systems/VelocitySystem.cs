using CSharpFunctionalExtensions;
using Dungeon.Game.Core.Components;
using Dungeon.Game.Core.Level;
using Dungeon.Game.Core.Level.Elements;
using Dungeon.Game.Core.Level.Utils;
using Dungeon.Game.Helper;
using SharpGDX;
using SharpGDX.Mathematics;

namespace Dungeon.Game.Core.Systems;

public sealed class VelocitySystem : System
{
    private const int DEFAULT_FRAME_TIME = 1;

    public VelocitySystem() : base(
        ComponentType.Is<VelocityComponent>(),
        ComponentType.Is<PositionComponent>())
    {
        
    }
    
    
    public override void Execute()
    {
        FilteredEntitiesF<VelocityComponent, PositionComponent>()
            .ForEach(UpdatePosition);
    }

    private void UpdatePosition(EntityComponentWrapper<VelocityComponent, PositionComponent> ec)
    {
        var entity = ec.Entity;
        var vc = ec.Component1;
        var pc = ec.Component2;
        
        var velocity = vc.CurrentVelocity.cpy();
        var maxSpeed = Math.Max(Math.Abs(vc.Velocity.x), Math.Abs(vc.Velocity.y));

        if (velocity.len() > maxSpeed)
        {
            velocity.nor();
            velocity.scl(maxSpeed);
        }

        if (GDX.Graphics is not null)
        {
            velocity.scl(GDX.Graphics.GetDeltaTime());
        }

        var newPosition = pc.Position.cpy().add(velocity);
        var hitWall = false;
        var canEnterOpenPits = vc.CanEnterOpenPits;


        var level = DungeonGame.CurrentLevel;
        if (level is null)
        {
            hitWall = true;
        }
        else if (IsAccessible(level.TileAt(newPosition), canEnterOpenPits))
        {
            hitWall = false;
            pc.Position = newPosition;
            MovementAnimation(entity, vc);
        }
        else if (IsAccessible(level.TileAt(new Vector2(newPosition.x, pc.Position.y)), canEnterOpenPits))
        {
            hitWall = true;
            pc.Position = new Vector2(newPosition.x, pc.Position.y);
            MovementAnimation(entity, vc);
            vc.CurrentVelocity.y = 0;
        }
        else if (IsAccessible(level.TileAt(new Vector2(pc.Position.x, newPosition.y)), canEnterOpenPits))
        {
            hitWall = true;
            pc.Position = new Vector2(pc.Position.x, newPosition.y);
            MovementAnimation(entity, vc);
            vc.CurrentVelocity.y = 0;
        }
        else
        {
            hitWall = true;
        }

        if (hitWall)
            vc.OnWallHitCallback(entity);

        var friction = (level?.TileAt(pc.Position)).GetFrictionOrDefault();
        vc.CurrentVelocity.scl(Math.Min(1.0f, 1.0f - friction));

        if (vc.CurrentVelocity.x < 0.01f)
            vc.CurrentVelocity.x = 0.0f;
        if (vc.CurrentVelocity.y < 0.01f)
            vc.CurrentVelocity.y = 0.0f;
        
    }

    private bool IsAccessible(Maybe<Tile> tile, bool canEnterPitTiles) =>
        tile.TryGetValue(out var t) 
        && (t.LevelElement.IsAccessible() 
            || canEnterPitTiles && t.LevelElement is LevelElement.Pit);

    // Why is the Velocity system setting the animation? This smells bad
    private void MovementAnimation(Entity entity, VelocityComponent velocity)
    {
        if (!entity.Fetch<DrawComponent>().TryGetValue(out var dc))
            return;

        // TODO
    }
}