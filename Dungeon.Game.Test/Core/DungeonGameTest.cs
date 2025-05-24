using Dungeon.Game.Core;
using Dungeon.Game.Core.Level.Elements;
using NSubstitute;
using Shouldly;

namespace Dungeon.Game.Test.Core;

[TestClass]
public class DungeonGameTest
{

    [TestCleanup]
    public void Cleanup()
    {
        DungeonGame.RemoveAllEntities();
        DungeonGame.CurrentLevel = null;
    }



    [TestMethod]
    public void AllEntities()
    {
        // Arrange
        DungeonGame.Add(new Entity());
        DungeonGame.Add(new Entity());
        DungeonGame.Add(new Entity());
        DungeonGame.Add(new Entity());

        var level = Substitute.For<ILevel>();
        DungeonGame.CurrentLevel = level;
        
        DungeonGame.Add(new Entity());
        DungeonGame.Add(new Entity());
        DungeonGame.Add(new Entity());
        DungeonGame.Add(new Entity());
        
        // Assert
        DungeonGame.Entities().Count().ShouldBe(8);
    }

    [TestMethod]
    public void RemoveAllEntities()
    {
        //Arrange
        DungeonGame.Add(new Entity());
        DungeonGame.Add(new Entity());
        DungeonGame.Add(new Entity());
        DungeonGame.Add(new Entity());

        var level = Substitute.For<ILevel>();
        DungeonGame.CurrentLevel = level;
        
        DungeonGame.Add(new Entity());
        DungeonGame.Add(new Entity());
        DungeonGame.Add(new Entity());
        DungeonGame.Add(new Entity());

        //Act
        DungeonGame.Entities().Count().ShouldBe(8);
        DungeonGame.RemoveAllEntities();
        //Assert
        
        DungeonGame.Entities().Count().ShouldBe(0);
    }

    [TestMethod]
    public void FindExisting()
    {
        //Arrange
        var entity = new Entity();
        var component = new DummyComponent();
        entity.Add(component);
        
        //Act
        DungeonGame.Add(entity);

        //Assert
        DungeonGame.Find(component).ShouldHaveValue(entity);

        DungeonGame.CurrentLevel = Substitute.For<ILevel>();
        DungeonGame.Find(component).ShouldHaveValue(entity);
    }

    [TestMethod]
    public void FindNonExisting()
    {
        //Arrange
        var component = new DummyComponent();

        //Act

        //Assert
        DungeonGame.Find(component).ShouldHaveNoValue();
    }
    
    private class DummyComponent : IComponent
    {
        
    }
}