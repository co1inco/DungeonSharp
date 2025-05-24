using SharpGDX.Graphics;

namespace Dungeon.Game.Core.Utils.Components.Draw;

public class TextureMap
{

    public static TextureMap Instance { get; } = new();

    private readonly Dictionary<string, Texture> _map = new();

    
    public Texture TextureAt(string path)
    {
        var fullPath = Path.GetFullPath(path);
        if (_map.TryGetValue(fullPath, out var texture))
            return texture;
        
        texture = new Texture(fullPath);
        _map.Add(fullPath, texture);
        return texture;
    }

}