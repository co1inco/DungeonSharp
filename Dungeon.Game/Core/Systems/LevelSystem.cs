using CSharpFunctionalExtensions;
using Dungeon.Game.Core.Components;
using Dungeon.Game.Core.Level;
using Dungeon.Game.Core.Level.Elements;
using Dungeon.Game.Core.Level.Elements.Tiles;
using Dungeon.Game.Core.Level.Generator;
using Dungeon.Game.Core.Level.Utils;
using Dungeon.Game.Core.Utils.Components.Draw;
using Dungeon.Game.Helper;
using Serilog;

namespace Dungeon.Game.Core.Systems;

public enum LevelSize
{
    Small,
    Medium,
    Large
}

public sealed class LevelSystem : System
{
    private static readonly ILogger Log = Serilog.Log.ForContext<DungeonGame>();
    
    private const float XOffset = 0.5f;
    private const float YOffset = 0.25f;
    private const string SoundEffect = "sounds/enterDoor.wav";
    
    private static LevelSize _levelSize = LevelSize.Medium;
    private static ILevel? _currentLevel;

    private readonly Action _onLevelLoad;
    private readonly Painter _painter;

    public LevelSystem(Painter painter, IGenerator generator, Action onLevelLoad) : base(
        ComponentType.Is<PlayerComponent>(),
        ComponentType.Is<PositionComponent>())
    {
        _painter = painter;
        Generator = generator;
        _onLevelLoad = onLevelLoad;
        OnEndTile = () => LoadLevel(_levelSize);
    }
    
    public IGenerator Generator { get; set; }
    
    public Action OnEndTile { get; set; }
    
    public static ILevel CurrentLevel => _currentLevel;

    public void LoadLevel(LevelSize size, DesignLabel label)
    {
        _currentLevel = Generator.Level(label, size);
        _onLevelLoad();
        Log.Information("A new level was loaded.");
    }
    
    public void LoadLevel(DesignLabel label) => 
        LoadLevel(_levelSize, label);

    public void LoadLevel(LevelSize size) => 
        LoadLevel(size, DesignLabel.Default); //TODO: random design

    public void LoadLevel(ILevel? level)
    {
        _currentLevel = level;
        _onLevelLoad();
        Log.Information("A new level was loaded.");
    }

    private void DrawLevel()
    {
        Dictionary<string, PainterConfig> mapping = new();

        foreach (var row in CurrentLevel.Layout)
        {
            foreach (var tile in row)
            {
                if (tile.LevelElement != LevelElement.Skip 
                    && !IsPitTileAndOpen(tile)
                    && tile.Visible)
                {
                    var path = tile.TexturePath;
                    if (!mapping.TryGetValue(path, out var config) || config.TintColor != tile.TintColor)
                    {
                        config = PainterConfig.WithTextureParam(path, XOffset, YOffset);
                        mapping[path] = config;
                    }
                    _painter.Draw(tile.Position, path, config);
                }
            }
        }
    }
    
    
    // private bool IsTilePitAndOpen(Tile tile)

    private bool IsOnOpenEndTile(Entity entity)
    {
        throw new NotImplementedException();
    }

    private bool IsPitTileAndOpen(Tile tile)
    {
        return tile is PitTile { IsOpen: true };
    }

    private Maybe<ILevel> IsOnDor(Entity entity)
    {
        throw new NotImplementedException();
    }

    private void PlaySound()
    {
        throw new NotImplementedException();
    }
    
    
    public override void Execute()
    {
        if (_currentLevel is null)
        {
            LoadLevel(_levelSize);
        }
        else if (FilteredEntities(
                     ComponentType.Is<PlayerComponent>() 
                     /*, ComponentType.Is<PositionComponent>()*/)
                 .Any(IsOnOpenEndTile))
        {
            OnEndTile();
        }
        else
        {
            FilteredEntities().ForEach(x =>
            {
                IsOnDor(x).Execute(level =>
                {
                    LoadLevel(level);
                    PlaySound();
                });
            });
        }
        DrawLevel();
    }

    public override void Stop()
    {
        // base.Stop(); - Level can not be paused
    }
}