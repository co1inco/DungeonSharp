using Dungeon.Game.Core.Level.Elements;
using Dungeon.Game.Core.Systems;

namespace Dungeon.Game.Core.Level.Generator;


public enum DesignLabel
{
    Default,
    Fire,
    Forest,
    Ice,
    Temple,
    Dark,
    Rainbow
}

public static class DesignLabelExtensions
{
    public static int Chance(this DesignLabel label) => label switch
    {
        DesignLabel.Default => 50,
        DesignLabel.Fire => 0,
        DesignLabel.Forest => 9,
        DesignLabel.Ice => 10,
        DesignLabel.Temple => 30,
        DesignLabel.Dark => 0,
        DesignLabel.Rainbow => 1,
        _ => throw new ArgumentOutOfRangeException(nameof(label), label, null)
    };
}


public interface IGenerator
{
    ILevel Level(DesignLabel label, LevelSize size);
}

public static class GeneratorExtensions
{
    public static ILevel Level(this IGenerator generator) => 
        generator.Level(DesignLabel.Default, LevelSize.Small); // TODO: should be random
    
    public static ILevel Level(this IGenerator generator, DesignLabel label) =>
        generator.Level(label, LevelSize.Small);
    
    public static ILevel Level(this IGenerator generator, LevelSize size) =>
        generator.Level(DesignLabel.Default, size);
}