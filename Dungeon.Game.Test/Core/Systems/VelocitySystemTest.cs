﻿using Dungeon.Game.Core;
using Dungeon.Game.Core.Components;
using Dungeon.Game.Core.Level;
using Dungeon.Game.Core.Level.Elements;
using Dungeon.Game.Core.Level.Generator;
using Dungeon.Game.Core.Level.Utils;
using Dungeon.Game.Core.Systems;
using Dungeon.Game.Core.Utils.Components.Path;
using NSubstitute;
using SharpGDX.Mathematics;
using SharpGDX.Utils;
using Shouldly;

namespace Dungeon.Game.Test.Core.Systems;

[TestClass]
public class VelocitySystemTest
{

    private ILevel _level;
    private DummyTile _tile;
    private readonly Vector2 _velocity = new Vector2(1, 2);
    private readonly Vector2 _startPosition = new Vector2(2, 4);
    private VelocitySystem _velocitySystem;
    private PositionComponent _positionComponet;
    private VelocityComponent _velocityComponet;
    private Entity _entity;
    
    
    [TestInitialize]
    public void Initialize()
    {
        _level = Substitute.For<ILevel>();
        _tile = new DummyTile(new Vector2(0,0));

        DungeonGame.Add(new LevelSystem(null!, null!, () => { }));
        DungeonGame.CurrentLevel = _level;
        _level.Layout.Returns([[_tile]]);
        _tile.SetFriction(0.75f); // If setter is not public, crete a dummyTile class instead
        _level.TileAt(Arg.Any<Coordinate>()).Returns(_tile);

        _velocitySystem = new VelocitySystem();
        DungeonGame.Add(_velocitySystem);
        
        _entity = new Entity();
        _velocityComponet = new VelocityComponent()
        {
            Velocity = _velocity,
        };
        _positionComponet = new PositionComponent
        {
            Position = _startPosition
        };
        _entity.Add(_velocityComponet);
        _entity.Add(_positionComponet);
        DungeonGame.Add(_entity);
    }

    [TestCleanup]
    public void Cleanup()
    {
        DungeonGame.RemoveAllEntities();
        DungeonGame.RemoveAllSystems();
        DungeonGame.CurrentLevel = null;
    }

    [TestMethod]
    public void UpdateValidMove()
    {
        //Arrange
        _tile.SetAccessible();
        _velocityComponet.Velocity = _velocity;

        // FIX: Testing a function by copy pasting its content is not real testing anything, is it... 
        var velocity = _velocityComponet.Velocity.cpy();
        if (velocity.len() > Math.Max(Math.Abs(_velocityComponet.Velocity.x), Math.Abs(_velocityComponet.Velocity.y)))
        {
            velocity.setLength(Math.Max(Math.Abs(_velocityComponet.Velocity.x), Math.Abs(_velocityComponet.Velocity.y)));
        }
        
        //Act
        _velocitySystem.Execute();
        
        //Assert
        _positionComponet.Position.x.ShouldBe(_startPosition.x + velocity.x, 0.001);
        _positionComponet.Position.y.ShouldBe(_startPosition.y + velocity.y, 0.001);
        
    }

    [TestMethod]
    public void UpdateValidMoveWithNegativeVelocity()
    {
        //Arrange
        _tile.SetAccessible();
        _velocityComponet.Velocity = _velocity.scl(-1.0f);

        // FIX: Testing a function by copy pasting its content is not real testing anything, is it... 
        var velocity = _velocityComponet.Velocity.cpy();
        if (velocity.len() > Math.Max(Math.Abs(_velocityComponet.Velocity.x), Math.Abs(_velocityComponet.Velocity.y)))
        {
            velocity.setLength(Math.Max(Math.Abs(_velocityComponet.Velocity.x), Math.Abs(_velocityComponet.Velocity.y)));
        }
        
        //Act
        _velocitySystem.Execute();
        
        //Assert
        _positionComponet.Position.x.ShouldBe(_startPosition.x + velocity.x, 0.001);
        _positionComponet.Position.y.ShouldBe(_startPosition.y + velocity.y, 0.001);
        
    }
    
    [TestMethod]
    public void UpdateInValidMove()
    {
        //Arrange
        _tile.SetUnAccessible();
        _velocityComponet.Velocity = _velocity;

        // FIX: Testing a function by copy pasting its content is not real testing anything, is it... 
        var velocity = _velocityComponet.Velocity.cpy();
        if (velocity.len() > Math.Max(Math.Abs(_velocityComponet.Velocity.x), Math.Abs(_velocityComponet.Velocity.y)))
        {
            velocity.setLength(Math.Max(Math.Abs(_velocityComponet.Velocity.x), Math.Abs(_velocityComponet.Velocity.y)));
        }
        
        //Act
        _velocitySystem.Execute();
        
        //Assert
        _positionComponet.Position.x.ShouldBe(_startPosition.x, 0.001);
        _positionComponet.Position.y.ShouldBe(_startPosition.y, 0.001);
        _velocityComponet.Velocity.x.ShouldBe(0);
        _velocityComponet.Velocity.y.ShouldBe(0);
    }
    
    [TestMethod]
    public void UpdateInValidNegativeMove()
    {
        //Arrange
        _tile.SetUnAccessible();
        _velocityComponet.Velocity = _velocity.scl(-1.0f);

        // FIX: Testing a function by copy pasting its content is not real testing anything, is it... 
        var velocity = _velocityComponet.Velocity.cpy();
        if (velocity.len() > Math.Max(Math.Abs(_velocityComponet.Velocity.x), Math.Abs(_velocityComponet.Velocity.y)))
        {
            velocity.setLength(Math.Max(Math.Abs(_velocityComponet.Velocity.x), Math.Abs(_velocityComponet.Velocity.y)));
        }
        
        //Act
        _velocitySystem.Execute();
        
        //Assert
        _positionComponet.Position.x.ShouldBe(_startPosition.x, 0.001);
        _positionComponet.Position.y.ShouldBe(_startPosition.y, 0.001);
        _velocityComponet.Velocity.x.ShouldBe(0);
        _velocityComponet.Velocity.y.ShouldBe(0);
    }
    
    
    
    private class DummyTile : Tile
    {
        public DummyTile(Vector2 position) 
            : base(new SimplePath("",0), position, DesignLabel.Default)
        {
        }

        public void SetAccessible() => LevelElement = LevelElement.Floor;

        public void SetUnAccessible() => LevelElement = LevelElement.Wall;

        public void SetFriction(float friction) => Friction = friction;
    }
}