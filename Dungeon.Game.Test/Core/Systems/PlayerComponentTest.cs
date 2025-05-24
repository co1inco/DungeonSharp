using Dungeon.Game.Core;
using Dungeon.Game.Core.Components;

namespace Dungeon.Game.Test.Core.Systems;

[TestClass]
public class PlayerComponentTest
{
    
    private PlayerComponent _player;

    [TestInitialize]
    public void Initialize()
    {
        _player = new PlayerComponent();
    }

    [TestMethod]
    public void AddFunction()
    {
        _player.RegisterCallback(1, _ => {}).ShouldHaveNoValue();     
    }

    [TestMethod]
    public void AddFunction_Existing()
    {
        //Arrange
        var originalCallback = (Entity _) => { };
        _player.RegisterCallback(1, originalCallback).ShouldHaveNoValue();

        //Act/Assert
        _player.RegisterCallback(1, _ => {}).ShouldHaveValue(originalCallback);
    }
    
}