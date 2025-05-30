using Dungeon.Game.Core.Utils.Components.Path;
using SharpGDX.Graphics;

namespace Dungeon.Game.Core.Utils.Components.Draw;

public class TextureMap
{

    public static TextureMap Instance { get; } = new();

    private readonly Dictionary<string, Texture> _map = new();

    
    public Texture TextureAt(IPath path)
    {
        if (_map.TryGetValue(path.Path, out var texture))
            return texture;
        
        texture = new Texture(path.Path);
        _map.Add(path.Path, texture);
        return texture;
    }

}