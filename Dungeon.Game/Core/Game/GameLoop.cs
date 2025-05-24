using CSharpFunctionalExtensions;
using Dungeon.Game.Core.Components;
using Dungeon.Game.Core.Level.Elements;
using Dungeon.Game.Core.Level.Generator;
using Dungeon.Game.Core.Systems;
using Dungeon.Game.Core.Utils;
using Dungeon.Game.Helper;
using Microsoft.VisualBasic;
using Serilog;
using SharpGDX;
using SharpGDX.Desktop;
using SharpGDX.Utils.Viewports;

namespace Dungeon.Game.Core;


public class Stage
{
    public void Act(double deltatime) {}
    public void Draw() {}
}

public class GameLoop : ScreenAdapter
{
    private static readonly ILogger Log = Serilog.Log.ForContext<GameLoop>();
    private static Stage? _stage;
    private bool _doSetup = true;
    private bool _newLevelWasLoadedInThisLoop = false;


    private void OnLevelLoad()
    {
        _newLevelWasLoadedInThisLoop = true;
        var hero = ECSManagement.Hero();
        var firstLoad = !ECSManagement.LevelStorageMap.ContainsKey(DungeonGame.CurrentLevel);
        hero.Execute(ECSManagement.Remove);
        
        // Remove the systems so that each triggerOnRemove(entity) will be called (basically cleanup).
        var s = ECSManagement.Systems.ToList();
        ECSManagement.RemoveAllSystems();
        ECSManagement.ActiveEntityStorage(
            ECSManagement.LevelStorageMap.ComputeIfAbsent(DungeonGame.CurrentLevel, _ => new HashSet<EntitySystemMapper>()));
        s.ForEach(x => ECSManagement.Add(x));

        try
        {
            hero.Execute(PlaceOnLevelStart);
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "Failed to place hero in new level");
        }

        hero.Execute(ECSManagement.Add);
        DungeonGame.CurrentLevel?.OnLoad();
        PreRunConfiguration.UserOnLevelLoad?.Invoke(firstLoad);
    }

    public static void Run()
    {
        var config = new DesktopApplicationConfiguration();
        config.SetWindowSizeLimits(PreRunConfiguration.WindowWidth, PreRunConfiguration.WindowHeight, 99999, 99999);
        config.SetForegroundFPS(PreRunConfiguration.FrameRate);
        config.SetResizable(PreRunConfiguration.Resizable);
        config.SetTitle(PreRunConfiguration.WindowTitle);
        // config.SetWindowIcon(PreRunConfiguration.LogoPath);
        config.DisableAudio(PreRunConfiguration.DisableAudio);
        // config.SetWindowListener(WindowEventManager.WindowListener);

        if (PreRunConfiguration.FullScreen)
        {
            config.SetFullscreenMode(DesktopApplicationConfiguration.GetDisplayMode());
        }
        else
        {
            config.SetWindowedMode(PreRunConfiguration.WindowWidth, PreRunConfiguration.WindowHeight);
        }
        
        _ = new DesktopApplication(new ApplicationListener(new GameLoop()), config);
    }


    public static Maybe<Stage> Stage() => _stage;

    private static void UpdateStage(Stage stage)
    {
        stage.Act(GDX.Graphics.GetDeltaTime());
        stage.Draw();
    }

    private static void SetupStage()
    {
        _stage = new Stage(/*TODO*/);
        // GDX.Input.SetInputProcessor(_stage);
    }

    private void Setup()
    {
        _doSetup = false;
        CreateSystems();
        SetupStage();
        PreRunConfiguration.UserOnSetup?.Invoke();
        DungeonGame.GetSystem<LevelSystem>().Execute(x => x.Execute());
    }
    
    public override void Render(float delta)
    {
        if (_doSetup)
            Setup();
        // DrawSystem
        // base.Render(delta);
        // TODO
    }

    private void CreateSystems()
    {
        // ECSManagement.Add(new PositionSystem())
        ECSManagement.Add(new CameraSystem());
        ECSManagement.Add(new LevelSystem(null!, new DummyGenerator(), OnLevelLoad));
        // ECSManagement.Add(new DrawSystem())
        // ECSManagement.Add(new VelocitySystem());
        // ECSManagement.Add(new PlayerSystem());
    }

    private class DummyGenerator : IGenerator
    {
        public ILevel Level(DesignLabel label, LevelSize size)
        {
            return new NullLevel();
        }
    }
    
    private void PlaceOnLevelStart(Entity entity) => throw new NotImplementedException();

    
    private class ApplicationListener(GameLoop gameLoop) : IApplicationListener
    {
        private IScreen? _screen;
        public IScreen? Screen
        {
            get => _screen;
            set
            {
                if (_screen is not null)
                {
                    _screen.Hide();
                }
                _screen = value;
            
                if (_screen is not null)
                {
                    _screen.Show();
                    _screen.Resize(GDX.Graphics.GetWidth(), GDX.Graphics.GetHeight());
                }
            }
        }

        public void Create()
        {
            Screen = gameLoop;
        }

        public void Dispose()
        {
            if (_screen is not null)
                _screen.Hide();
        }

        public void Pause()
        {
            if (_screen is not null)
                _screen.Pause();
        }
        
        public void Resume()
        {
            if (_screen is not null)
                _screen.Resume();
        }

        public void Render()
        {
            if (_screen is not null)
                _screen.Render(GDX.Graphics.GetDeltaTime());
        }

        public void Resize(int width, int height)
        {
            if (_screen is not null)
                _screen.Resize(width, height);
        }

    }
}