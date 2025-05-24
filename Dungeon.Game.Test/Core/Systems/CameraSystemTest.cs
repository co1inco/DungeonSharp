using Dungeon.Game.Core;
using Dungeon.Game.Core.Components;
using Dungeon.Game.Core.Level;
using Dungeon.Game.Core.Level.Elements;
using Dungeon.Game.Core.Systems;
using NSubstitute;
using SharpGDX.Graphics;
using SharpGDX.Mathematics;
using SharpGDX.Mathematics.Collision;
using SharpGDX.Scenes.Scene2D.Utils;
using Shouldly;

namespace Dungeon.Game.Test.Core.Systems;

[TestClass]
public class CameraSystemTest
{

    private static readonly Vector2 TestPoint = new Vector2(3, 3);

    private ILevel _level;
    private Tile _tile;
    private CameraSystem _cameraSystem;
    private Vector2 _expectedFocusPoint;
        

    [TestInitialize]
    public void Initialize()
    {
        _tile = Substitute.For<Tile>(TestPoint);
        _level = Substitute.For<ILevel>();
        _level.StartTile.Returns(_tile);
        _cameraSystem = new CameraSystem();
        DungeonGame.Add(_cameraSystem);
        DungeonGame.Add(new LevelSystem(null!, null!, () => { }));
    }


    [TestCleanup]
    public void Cleanup()
    {
        DungeonGame.RemoveAllEntities();
        DungeonGame.CurrentLevel = null;
        DungeonGame.RemoveAllSystems();
    }


    [TestMethod]
    public void ExecuteWithEntity()
    {
        //Arrange
        DungeonGame.CurrentLevel = _level;
        var entity = new Entity();
        _expectedFocusPoint = new Vector2(3, 3);
        var positionComponent = new PositionComponent() { Position = _expectedFocusPoint };
        entity.Add(positionComponent);
        DungeonGame.Add(entity);

        //Act
        _cameraSystem.Execute();
        
        //Assert
        CameraSystem.Camera.position.x.ShouldBe(_expectedFocusPoint.x, 0.001f);
        CameraSystem.Camera.position.y.ShouldBe(_expectedFocusPoint.y, 0.001f);
    }


    [TestMethod]
    public void ExecuteWithoutEntity()
    {
        //Arrange
        DungeonGame.CurrentLevel = _level;

        //Act
        _cameraSystem.Execute();

        //Assert
        CameraSystem.Camera.position.x.ShouldBe(TestPoint.x, 0.001f);
        CameraSystem.Camera.position.y.ShouldBe(TestPoint.y, 0.001f);
    }

    [TestMethod]
    public void ExecuteWithoutLevel()
    {
        //Arrange
        DungeonGame.CurrentLevel = null;
        _expectedFocusPoint = new Vector2(0, 0);

        //Act
        _cameraSystem.Execute();

        //Assert
        CameraSystem.Camera.position.x.ShouldBe(_expectedFocusPoint.x, 0.001f);
        CameraSystem.Camera.position.y.ShouldBe(_expectedFocusPoint.y, 0.001f);
    }

    [Ignore("Outcome is random")]
    [TestMethod]
    public void IsPointInFrustumWithVisiblePoint()
    {
        //Assert
        CameraSystem.IsPointInFrustum(1, 1).ShouldBeTrue();
    }
    
    [Ignore("Outcome is random")]
    [TestMethod]
    public void IsPointInFrustumWithInvisiblePoint()
    {
        //Assert
        // CameraSystem.IsPointInFrustum(100, 100).ShouldBeFalse();
        CameraSystem.IsPointInFrustum(1000, 1000).ShouldBeFalse();
    }
}