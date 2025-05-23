using CSharpFunctionalExtensions;
using Dungeon.Game.Core;
using NSubstitute;
using Shouldly;
using Entity = Dungeon.Game.Core.Entity;

namespace Dungeon.Game.Test.Core;

[TestClass]
public class SystemTest
{
    private DummySystem _testSystem;
    
    [TestInitialize]
    public void Initialize()
    {
        _testSystem = new DummySystem();
    }

    [TestMethod]
    public void Add()
    {
        //Arrange
        var e = new Entity();

        //Act
        _testSystem.TriggerOnAdd(e);

        //Assert
        _testSystem.OnAddTriggered.ShouldBeTrue();
    }
    
    [TestMethod]
    public void Remove()
    {
        //Arrange
        var e = new Entity();

        //Act
        _testSystem.TriggerOnRemove(e);

        //Assert
        _testSystem.OnRemoveTriggered.ShouldBeTrue();
    }

    [TestMethod]
    public void FilteredEntities_NoParameters()
    {
        //Arrange
        var entity1 = new Entity();
        entity1.Add(new DummyComponent());
        var entity2 = new Entity();
        DungeonGame.Add(entity1);
        DungeonGame.Add(entity2);
        
        //Act
        var entities = _testSystem
            .FilteredEntities()
            .ToList();

        //Assert
        entities.ShouldContain(entity1);
        entities.ShouldNotContain(entity2);
    }
    
    [TestMethod]
    public void FilteredEntities_ArrayParameters()
    {
        //Arrange
        var entity1 = new Entity();
        entity1.Add(new DummyComponent());
        var entity2 = new Entity();
        DungeonGame.Add(entity1);
        DungeonGame.Add(entity2);
        
        //Act
        var entities = _testSystem
            .FilteredEntities(ComponentType.Is<DummyComponent>())
            .ToList();

        //Assert
        entities.ShouldContain(entity1);
        entities.ShouldNotContain(entity2);
    }
    
    
    

    private class DummySystem : Game.Core.System
    {
        public bool OnAddTriggered { get; private set; }
        public bool OnRemoveTriggered { get; private set; }

        public DummySystem()
        {
            OnEntityAdd = _ => OnAddTriggered = true;
            OnEntityRemove = _ => OnRemoveTriggered = true;
        }
        
        public override void Execute()
        {
            
        }
    }
    
    private class DummyComponent : IComponent {}
}