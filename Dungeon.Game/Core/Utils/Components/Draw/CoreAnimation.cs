using Dungeon.Game.Core.Utils.Components.Path;

namespace Dungeon.Game.Core.Utils.Components.Draw;

public record CoreAnimation(string Path, int Priority) : IPath
{
    public static readonly CoreAnimation Idle = new("idle", CoreAnimationProperties.IDLE.Priority);
    public static readonly CoreAnimation IdleLeft = new("idle_left", CoreAnimationProperties.IDLE.Priority);
    public static readonly CoreAnimation IdleRight = new("idle_right", CoreAnimationProperties.IDLE.Priority);
    public static readonly CoreAnimation IdleUp = new("idleUp", CoreAnimationProperties.IDLE.Priority);
    public static readonly CoreAnimation IdleDown = new("idleDown", CoreAnimationProperties.IDLE.Priority);
    
    public static readonly CoreAnimation Run = new("run", CoreAnimationProperties.RUN.Priority);
    public static readonly CoreAnimation RunLeft = new("run_left", CoreAnimationProperties.RUN.Priority);
    public static readonly CoreAnimation RunRight = new("run_right", CoreAnimationProperties.RUN.Priority);
    public static readonly CoreAnimation RunUp = new("run_up", CoreAnimationProperties.RUN.Priority);
    public static readonly CoreAnimation RunDown = new("run_down", CoreAnimationProperties.RUN.Priority);
}