using Dungeon.Game.Core.Utils.Components.Path;
using SharpGDX.Graphics;

namespace Dungeon.Game.Core.Utils.Components.Draw;

public record PainterConfig(
    float XOffset,
    float YOffset,
    float XScaling,
    float YScaling,
    int TintColor = -1
)
{
    public static PainterConfig WithTextureParam(
        IPath path,
        float xOffset,
        float yOffset,
        int tintColor = -1) =>
        WithTextureParams(xOffset, yOffset, 1, TextureMap.Instance.TextureAt(path), tintColor);

    private static PainterConfig WithTextureParams(
        float xOffset,
        float yOffset,
        float xScaling,
        Texture texture,
        int tintColor = -1) =>
        new(
            xOffset,
            yOffset,
            xScaling,
            ((float) texture.getHeight() / (float) texture.getWidth()), 
            tintColor 
        );
}