using Dungeon.Game.Core.Utils.Components.Path;
using Serilog;

namespace Dungeon.Game.Core.Utils;

/// <summary>
/// Helps locate resources / assets. 
/// </summary>
public class ResourceLoader
{
    private static readonly ILogger Log = Serilog.Log.ForContext<ResourceLoader>();

    // TODO: these should be user overridable.
    //  Maybe add a static Instance that can be set by the user and make the methods use that
    

    // public static IEnumerable<IPath> FindFiles(IPath path)
    // {
    //     var absPath = Path.IsPathRooted(path.Path) 
    //         ? path.Path 
    //         : Path.Combine("Assets", path.Path); 
    // }

    public static IEnumerable<(string Key, IPath[] Files)> GetResourcesIn(IPath path, string searchPattern = "*")
    {
        var p = FindDirectory(path.Path);

        return Directory.GetDirectories(p).Select(x =>
        {
            var key = x.Replace(p, "").TrimStart("\\/.".ToArray());
            var files = Directory
                .GetFiles(x, searchPattern)
                .Select(IPath (y) => new SimplePath(y))
                .ToArray();
            return (key, files);
        });

    }

    /// <summary>
    /// Find the  location of an asset directory 
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string FindDirectory(string path)
    {
        if (Directory.Exists(path))
            return path;
        
        // Check if the resources are inside the assets director relative to the current directory
        var p = Path.Combine("Assets", path);
        if (Directory.Exists(p))
            return p;

        // TODO: check if the resources are inside the assembly
        
        throw new DirectoryNotFoundException($"Could not locate asset directory: {path}");
    }
}