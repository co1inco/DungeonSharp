using Dungeon.Game.Core.Components;
using Dungeon.Game.Core.Level.Elements;
using Dungeon.Game.Helper;
using SharpGDX.Mathematics;

namespace Dungeon.Game.Core.Systems;

public class PositionSystem() : System(ComponentType.Is<PositionComponent>())
{
    public override void Execute()
    {
        FilteredEntitiesF<PositionComponent>()
            .Where(x => x.Component.Position.Equals(PositionComponent.IllegalPosition))
            .ForEach(RandomPosition);
    }


    private void RandomPosition(EntityComponentWrapper<PositionComponent> entity)
    {
        if (DungeonGame.CurrentLevel?.FreePosition().TryGetValue(out var position) ?? false)
        {
            position.x += 0.5f;
            position.y += 0.5f;
            entity.Component.Position = position;
        }
    }
}