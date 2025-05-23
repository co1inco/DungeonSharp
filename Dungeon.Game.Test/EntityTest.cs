using System.ComponentModel;
using Dungeon.Game.Core;
using FluentAssertions;
using NSubstitute;
using Shouldly;
using IComponent = Dungeon.Game.Core.IComponent;

namespace Dungeon.Game.Test;

[TestClass]
public class EntityTest
{
    private DummyComponent _testComponent;
    private Entity _entity;
    
    
    [TestInitialize]
    public void Initialize()
    {
        _testComponent = new DummyComponent();
        
        _entity = new Entity();
        _entity.Add(_testComponent);
    }

    [TestMethod]
    public void AddComponent()
    {
        _entity.Fetch<DummyComponent>()
            .ShouldHaveValue(_testComponent);
    }

    [TestMethod]
    public void AddExistingComponent()
    {
        //Arrange
        var newComponent = new DummyComponent();

        //Act
        _entity.Add(newComponent);

        //Assert
        _entity.Fetch<DummyComponent>().ShouldHaveValue(newComponent);
        _entity.Components.ShouldContain(newComponent);
        _entity.Components.ShouldHaveSingleItem();
    }

    [TestMethod]
    public void FetchComponent()
    {
        //Assert
        _entity.Fetch<DummyComponent>().ShouldHaveValue(_testComponent);
        _entity.Fetch(typeof(DummyComponent)).ShouldHaveValue(_testComponent);
    }

    [TestMethod]
    public void FetchNonExistentComponent()
    {
        //Arrange
        var entity = new Entity();

        //Assert
        entity.Fetch<DummyComponent>().ShouldHaveNoValue();
        entity.Fetch(typeof(DummyComponent)).ShouldHaveNoValue();
    }
    
    [TestMethod]
    public void RemoveComponent()
    {
        //Act
        _entity.Remove(typeof(DummyComponent));
        
        //Assert
        _entity.Fetch<DummyComponent>().ShouldHaveNoValue();
    }
    
    [TestMethod]
    public void RemoveComponent_Generic()
    {
        //Act
        _entity.Remove<DummyComponent>();
        
        //Assert
        _entity.Fetch<DummyComponent>().ShouldHaveNoValue();
    }

    [TestMethod]
    public void HasComponent()
    {
        //Assert
        _entity.IsPresent<DummyComponent>().ShouldBeTrue();
        _entity.IsPresent(typeof(DummyComponent)).ShouldBeTrue();
    }
    
    [TestMethod]
    public void CompareToSameId()
    {
        _entity.Equals(_entity).ShouldBeTrue();
        // Equals(_entity, _entity).ShouldBeTrue();
        _entity.CompareTo(_entity).ShouldBe(0);
        (_entity >= _entity).ShouldBeTrue();
    }

    [TestMethod]
    public void CompareToLowerId()
    {
        //Arrange
        var entity1 = new Entity();
        var entity2 = new Entity();

        //Act / Assert
        entity1.Equals(entity2).ShouldBeFalse();
        (entity1 < entity2).ShouldBeTrue();
        (entity1 <= entity2).ShouldBeTrue();
        entity1.CompareTo(entity2).ShouldBeLessThan(0);
    }
    
    [TestMethod]
    public void CompareToHigherId()
    {
        //Arrange
        var entity1 = new Entity();
        var entity2 = new Entity();

        //Act / Assert
        entity2.Equals(entity1).ShouldBeFalse();
        (entity2 > entity1).ShouldBeTrue();
        (entity2 >= entity1).ShouldBeTrue();
        entity2.CompareTo(entity1).ShouldBeGreaterThan(0);
    }
    
    
    
    private class DummyComponent() : IComponent {}
}