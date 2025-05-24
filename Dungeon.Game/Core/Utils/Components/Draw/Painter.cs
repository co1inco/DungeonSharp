using System.Drawing;
using Dungeon.Game.Core.Systems;
using SharpGDX.Graphics.G2D;

namespace Dungeon.Game.Core.Utils.Components.Draw;

public record PainterConfig(
    float XOffset,
    float YOffset,
    float XScaling,
    float YScaling,
    int TintColor = -1
);

public class Painter
{
    private readonly SpriteBatch _batch;

    public Painter(SpriteBatch batch)
    {
        _batch = batch;
    }

    public void Draw(Point position, string texturePath, PainterConfig config)
    {
        float realX = position.X + config.XOffset;
        float realY = position.Y + config.YOffset;

        if (!CameraSystem.IsPointInFrustum(realX, realX))
            return;
        
        var sprite = new Sprite(TextureMap.Instance.TextureAt(texturePath));
        sprite.SetSize(config.XScaling, config.YScaling);
        sprite.SetPosition(realX, realY);
        
        _batch.Begin();

        if (config.TintColor != -1)
        { 
            var color = SharpGDX.Graphics.Color.Clear;
            SharpGDX.Graphics.Color.RGB888ToColor(color, config.TintColor);
            sprite.SetColor(color);
        }
        
        sprite.Draw(_batch);
        
        _batch.End();
    }
    
}