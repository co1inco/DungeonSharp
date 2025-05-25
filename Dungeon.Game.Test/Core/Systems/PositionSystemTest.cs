using Dungeon.Game.Core;
using Dungeon.Game.Core.Components;
using Dungeon.Game.Core.Level.Elements;
using Dungeon.Game.Core.Level.Elements.Tiles;
using Dungeon.Game.Core.Level.Generator;
using Dungeon.Game.Core.Systems;
using NSubstitute;
using SharpGDX.Mathematics;
using Shouldly;

namespace Dungeon.Game.Test.Core.Systems;

[TestClass]
public class PositionSystemTest
{
    private PositionComponent _positionCompoent;
    private PositionSystem _positionSystem;
    private Entity _entity;
    
    
    [TestInitialize]
    public void Initialize()
    {
        _positionCompoent = new PositionComponent();
        _positionSystem = new PositionSystem();
        _entity = new Entity();
        _entity.Add(_positionCompoent);

        DungeonGame.Add(_entity);
        DungeonGame.Add(_positionSystem);
        DungeonGame.Add(new LevelSystem(null!, null!, () => { }));

        DungeonGame.CurrentLevel = Substitute.For<ILevel>();
        DungeonGame.CurrentLevel.Layout.Returns([[new FloorTile("", new Vector2(0, 0), DesignLabel.Default)]]);
    }

    [TestCleanup]
    public void Cleanup()
    {
        DungeonGame.RemoveAllEntities();
        DungeonGame.RemoveAllSystems();
        DungeonGame.CurrentLevel = null;
    }


    [TestMethod]
    public void LegalPosition()
    {
        //Arrange
        _positionCompoent.Position = Vector2.Zero;

        //Act
        _positionSystem.Execute();

        //Assert
        _positionCompoent.Position.ShouldBe(Vector2.Zero);
    }

    [TestMethod]
    public void IllegalPosition()
    {
        //Arrange
        _positionCompoent.Position = PositionComponent.IllegalPosition;

        //Act
        _positionSystem.Execute();

        //Assert
        _positionCompoent.Position.ShouldNotBe(PositionComponent.IllegalPosition);
    }
    
}