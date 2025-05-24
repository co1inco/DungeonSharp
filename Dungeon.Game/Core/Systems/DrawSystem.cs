using System.Diagnostics;
using Dungeon.Game.Core.Components;
using Dungeon.Game.Core.Utils.Components.Draw;
using Dungeon.Game.Helper;
using SharpGDX.Graphics.G2D;

namespace Dungeon.Game.Core.Systems;

public class DrawSystem : System
{
    public static SpriteBatch Batch { get; } = new();
    public static Painter Painter { get; } = new Painter(Batch);

    private readonly Dictionary<string, PainterConfig> _configs = new();
    
    
    public override void Execute()
    {
        FilteredEntities<DrawComponent, PositionComponent>()
            .ForEach(x =>
            {
                if (!x.IsPresent<PlayerComponent>() && !ShouldDraw(x))
                    return;
                Draw(BuildDataObject(x));
            });
    }

    private bool ShouldDraw(Entity entity)
    {
        // TODO
        return true;
    }

    private void Draw(DsData dsd)
    {
        ReduceFrameTimer(dsd.Draw);
        SetNextAnimation(dsd.Draw);

        throw new NotImplementedException();
    }

    private void SetNextAnimation(DrawComponent dsdDraw)
    {
        throw new NotImplementedException();
    }

    private void ReduceFrameTimer(DrawComponent dsdDraw)
    {
        throw new NotImplementedException();
    }

    private DsData BuildDataObject(Entity entity)
    {
        var dc = entity.Fetch<DrawComponent>().GetValueOrThrow(new UnreachableException("Should have ben filtered out")); 
        var pc = entity.Fetch<PositionComponent>().GetValueOrThrow(new UnreachableException("Should have ben filtered out"));
        return new DsData(entity, dc, pc);
    }

    private record DsData(Entity Entity, DrawComponent Draw, PositionComponent Position);
}