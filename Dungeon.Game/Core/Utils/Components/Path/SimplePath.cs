namespace Dungeon.Game.Core.Utils.Components.Path;

public record SimplePath(string Path, int Priority = 0) : IPath;