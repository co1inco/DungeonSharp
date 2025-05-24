using System.Drawing;
using CSharpFunctionalExtensions;
using Dungeon.Game.Core.Components;
using SharpGDX;
using SharpGDX.Graphics;
using SharpGDX.Mathematics;
using SharpGDX.Mathematics.Collision;

namespace Dungeon.Game.Core.Systems;

public class CameraSystem : System
{
    public const float DEFAULT_ZOOM_FACTOR = 0.35f;
    private const float FIELD_WIDTH_AND_HEIGHT_IN_PX = 16f;
    public static OrthographicCamera Camera { get; } = new(ViewportWidth(), ViewportHeight())
    {
        zoom = DEFAULT_ZOOM_FACTOR
    };
    
    
    private static float ViewportWidth() => 
        PreRunConfiguration.WindowWidth / FIELD_WIDTH_AND_HEIGHT_IN_PX; // Using PreRun to keep the same zoom

    private static float ViewportHeight() => 
        PreRunConfiguration.WindowHeight / FIELD_WIDTH_AND_HEIGHT_IN_PX; // Using PreRun to keep the same zoom

    public static bool IsPointInFrustum(float x, float y)
    {
        return true;
        // Not working for some reason
        var offset = 1f;
        // var bounds = new BoundingBox(new Vector3(x- offset, y - offset, 0), new Vector3(x + offset, y + offset, 1));
        var bounds = new BoundingBox(new Vector3(x - offset, y - offset, 0), new Vector3(x + offset, y + offset, 0));
        return Camera.frustum.boundsInFrustum(bounds);
    }


    public CameraSystem() : base(
        ComponentType.Is<CameraComponent>(),
        ComponentType.Is<PositionComponent>())
    {
        
    }
    
    
    public override void Execute()
    {
        FilteredEntities<CameraComponent, PositionComponent>()
            .TryFirst()
            .Match(Focus, Focus);

        if (GDX.Graphics is not null)
        {
            var aspectRation = (GDX.Graphics.GetWidth() / (float)GDX.Graphics.GetHeight());
            Camera.viewportWidth = ViewportWidth();
            Camera.viewportHeight = ViewportHeight() / aspectRation;
        }
        
        Camera.update();
    }

    private void Focus()
    {
        var startTile = DungeonGame.StartTile();
        var point = DungeonGame.CurrentLevel is null || startTile is null
            ? new Vector2(0, 0)
            : startTile.Position;    
        Focus(point);
    }

    private void Focus(Entity entity)
    {
        var pc = entity.Fetch<PositionComponent>()
            .GetValueOrThrow(new Exception($"Missing component: {nameof(PositionComponent)}"));
        Focus(pc.Position);
    }

    private void Focus(Vector2 point) => 
        Camera.position.Set(point, 0);
}