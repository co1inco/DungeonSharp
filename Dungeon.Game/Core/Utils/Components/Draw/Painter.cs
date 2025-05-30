using System.Drawing;
using Dungeon.Game.Core.Systems;
using Dungeon.Game.Core.Utils.Components.Path;
using SharpGDX.Graphics;
using SharpGDX.Graphics.G2D;
using SharpGDX.Mathematics;

namespace Dungeon.Game.Core.Utils.Components.Draw;

public class Painter
{
    private readonly SpriteBatch _batch;

    public Painter(SpriteBatch batch)
    {
        _batch = batch;
    }

    public void Draw(Vector2 position, IPath texturePath, PainterConfig config)
    {
        float realX = position.x + config.XOffset;
        float realY = position.y + config.YOffset;

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