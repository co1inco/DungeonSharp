using Dungeon.Game.Core.Components;
using Dungeon.Game.Core.Utils.Components.Draw;
using Dungeon.Game.Core.Utils.Components.Path;
using FluentAssertions;
using Shouldly;

namespace Dungeon.Game.Test.Core.Systems;

[TestClass]
public class DrawComponentTest
{
    private readonly IPath _animationPath = new SimplePath("textures/test_hero");
    private DrawComponent _drawComponent = null!;

    [TestInitialize]
    public void Initialize()
    {
        _drawComponent = new DrawComponent(_animationPath);
    }


    [TestMethod]
    public void CurrentAnimation()
    {
        _drawComponent.IsCurrentAnimation(CoreAnimation.IdleLeft).ShouldBeTrue();

        _drawComponent.CurrentAnimation(CoreAnimation.IdleRight);
        
        _drawComponent.IsCurrentAnimation(CoreAnimation.IdleRight).ShouldBeTrue();
    }

    [TestMethod]
    public void CurrentAnimationWithMultiplePaths()
    {
        // IDLE_DOWN and IDLE_UP don't exist, so IDLE_LEFT is expected
        _drawComponent.CurrentAnimation(CoreAnimation.IdleDown, CoreAnimation.IdleUp, CoreAnimation.IdleLeft);
        _drawComponent.IsCurrentAnimation(CoreAnimation.IdleLeft).ShouldBeTrue();
        
        // Take the first valid animation
        _drawComponent.CurrentAnimation(CoreAnimation.IdleRight, CoreAnimation.IdleUp, CoreAnimation.IdleLeft);
        _drawComponent.IsCurrentAnimation(CoreAnimation.IdleRight).ShouldBeTrue();
    }

    [TestMethod]
    public void CurrentAnimationWithMultiplePathsAllValid()
    {
        _drawComponent.CurrentAnimation(CoreAnimation.IdleRight, CoreAnimation.IdleLeft);
        _drawComponent.IsCurrentAnimation(CoreAnimation.IdleRight).ShouldBeTrue();
    }
    
    [TestMethod]
    public void CurrentAnimationWithMultiplePathsNoValid()
    {
        //Arrange
        _drawComponent.CurrentAnimation(CoreAnimation.IdleRight);
        _drawComponent.IsCurrentAnimation(CoreAnimation.IdleRight).ShouldBeTrue();
        
        //Act
        _drawComponent.CurrentAnimation(CoreAnimation.IdleUp, CoreAnimation.IdleDown);
        
        //Assert
        _drawComponent.IsCurrentAnimation(CoreAnimation.IdleRight).ShouldBeTrue();
    }

    [TestMethod]
    public void GetAnimations()
    {
        _drawComponent.Animation(CoreAnimation.RunLeft).ShouldHaveSomeValue();
    }

    [TestMethod]
    public void HasAnimations()
    {
        _drawComponent.HasAnimation(CoreAnimation.RunLeft).ShouldBeTrue();
        _drawComponent.HasAnimation(CoreAnimation.RunDown).ShouldBeFalse();
    }
    
}