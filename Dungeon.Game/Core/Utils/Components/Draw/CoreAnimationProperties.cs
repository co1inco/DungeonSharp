using System.Transactions;
// ReSharper disable InconsistentNaming

namespace Dungeon.Game.Core.Utils.Components.Draw;

public record CoreAnimationProperties(int Priority)
{
    public static readonly CoreAnimationProperties IDLE = new Idle();
    public static readonly CoreAnimationProperties RUN = new Run();
    public static readonly CoreAnimationProperties DEFAULT = new Default();
    public record Idle() : CoreAnimationProperties(1000);
    public record Run() : CoreAnimationProperties(2000);
    public record Default() : CoreAnimationProperties(0);
}