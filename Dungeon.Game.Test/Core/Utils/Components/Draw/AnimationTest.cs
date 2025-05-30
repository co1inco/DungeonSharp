using Dungeon.Game.Core.Utils.Components.Draw;
using Dungeon.Game.Core.Utils.Components.Path;
using Dungeon.Game.Helper;
using Shouldly;

namespace Dungeon.Game.Test.Core.Utils.Components.Draw;

[TestClass]
public class AnimationTest
{
    private readonly IPath _a = new SimplePath("a"); 
    private readonly IPath _b = new SimplePath("b"); 
    private readonly IPath _c = new SimplePath("c"); 
    private readonly IPath _d = new SimplePath("d"); 

    [TestMethod]
    public void ConstructorShouldThrow()
    {
        Should.Throw<ArgumentException>(() => Animation.FromCollection([]));
    }


    [TestMethod]
    public void GetNextAnimationFrame()
    {
        //Arrange
        var animation = Animation.FromCollection([_a, _b, _c], frameTimes: 10);

        //Act
        
        //Assert
        animation.NextAnimationTexturePath().ShouldBe(_a);
        Enumerable.Repeat(0, 10).ForEach(_ => animation.NextAnimationTexturePath());
        animation.NextAnimationTexturePath().ShouldBe(_b);
        Enumerable.Repeat(0, 10).ForEach(_ => animation.NextAnimationTexturePath());
        animation.NextAnimationTexturePath().ShouldBe(_c);
        Enumerable.Repeat(0, 10).ForEach(_ => animation.NextAnimationTexturePath());
        animation.NextAnimationTexturePath().ShouldBe(_a);
        Enumerable.Repeat(0, 10).ForEach(_ => animation.NextAnimationTexturePath());
    }

    [TestMethod]
    public void IsFinished_NoMoreFrameAndNotLooping()
    {
        //Arrange
        var animation = Animation.FromCollection([_a, _b], frameTimes: 1, looping: false);

        animation.IsFinished().ShouldBeFalse();
        
        //Act
        animation.NextAnimationTexturePath();
        
        //Assert
        animation.IsFinished().ShouldBeTrue();
    }
    
    [TestMethod]
    public void IsFinished_OneMoreFrameAndNotLooping()
    {
        //Arrange
        var animation = Animation.FromCollection([_a, _b], frameTimes: 1, looping: false);

        //Assert
        animation.IsFinished().ShouldBeFalse();
    }
    
    [TestMethod]
    public void IsFinished_NoMoreFrameAndLooping()
    {
        //Arrange
        var animation = Animation.FromCollection([_a, _b], frameTimes: 1, looping: true);

        animation.IsFinished().ShouldBeFalse();
        
        //Act
        animation.NextAnimationTexturePath();
        
        //Assert
        animation.IsFinished().ShouldBeFalse();
    }

    [TestMethod]
    public void NextAnimation_NoLoopingFrameTime1()
    {
        //Arrange
        var animation = Animation.FromCollection([_a, _b], frameTimes: 1, looping: false);

        //Act

        //Assert
        animation.NextAnimationTexturePath().ShouldBe(_a);
        animation.NextAnimationTexturePath().ShouldBe(_b);
        animation.NextAnimationTexturePath().ShouldBe(_b);
    }
    
    [TestMethod]
    public void NextAnimation_NoLoopingFrameTime2()
    {
        //Arrange
        var animation = Animation.FromCollection([_a, _b], frameTimes: 2, looping: false);

        //Act

        //Assert
        animation.NextAnimationTexturePath().ShouldBe(_a);
        animation.NextAnimationTexturePath().ShouldBe(_a);
        animation.NextAnimationTexturePath().ShouldBe(_b);
        animation.NextAnimationTexturePath().ShouldBe(_b);
    }
}