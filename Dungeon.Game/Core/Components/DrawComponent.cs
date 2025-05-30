using CSharpFunctionalExtensions;
using Dungeon.Game.Core.Utils;
using Dungeon.Game.Core.Utils.Components.Draw;
using Dungeon.Game.Core.Utils.Components.Path;
using Dungeon.Game.Helper;
using Serilog;
using Serilog.Core;

namespace Dungeon.Game.Core.Components;

public sealed class DrawComponent : IComponent
{
    private static readonly ILogger Log = Serilog.Log.ForContext<DrawComponent>();
    
    private readonly OrderedDictionary<string, Animation> _animationMap = new OrderedDictionary<string, Animation>(); 
    private Animation? _currentAnimation;
    private int _tintColor = -1;
    private bool _isVisible = true;

    
    public DrawComponent(IPath path)
    {
        LoadAnimationAssets(path);
        CurrentAnimation(
            CoreAnimation.IdleDown,
            CoreAnimation.IdleLeft,
            CoreAnimation.IdleRight,
            CoreAnimation.IdleUp,
            CoreAnimation.Idle
        );

        if (_currentAnimation is null)
        {
            _animationMap[CoreAnimation.Idle.Path] = Utils.Components.Draw.Animation.DefaultAnimation();
            CurrentAnimation(CoreAnimation.Idle);
        }
    }

    
    public IDictionary<IPath, int> AnimationQueue { get; } = new SortedDictionary<IPath, int>();

    public IReadOnlyDictionary<string, Animation> AnimationMap => _animationMap;

    
    public Animation? CurrentAnimation() => _currentAnimation;

    
    // public void CurrentAnimation(IPath animationName) =>
    //     CurrentAnimation([animationName]);

    public void CurrentAnimation(params IEnumerable<IPath> animationNames)
    {
        foreach (var animation in animationNames)
        {
            if (_animationMap.TryGetValue(animation.Path, out var value))
            {
                _currentAnimation = value;
                break;
            }
            else
            {
                Log.Warning("Animation {Animation} was not found", animation);
            }
        }
    }

    public void CurrentAnimation(string animationName)
    {
        if (_animationMap.TryGetValue(animationName, out var animation))
            _currentAnimation = animation;
        else
            Log.Warning("Animation with name {Animation} was not found", animationName);
    }

    public void QueueAnimation(IEnumerable<IPath> animations, int forFrames) => 
        QueueAnimation(animations, a => AnimationQueue[a] = Duration(a, forFrames));

    public void QueueAnimation(IEnumerable<IPath> animations) => 
        QueueAnimation(
            animations, 
            a => AnimationQueue[a] = Duration(a, Animation(a).GetValueOrThrow().Duration));


    private void QueueAnimation(IEnumerable<IPath> animations, Action<IPath> fn)
    {
        if (animations.FirstOrDefault(HasAnimation) is {} animation)
            fn(animation);
    }

    private int Duration(IPath path, int forFrames) =>
        Math.Max(
            AnimationQueue.TryGetValue(path, out var duration) ? duration : 0,
            forFrames
        );

    public void DeQueue(IPath path) => 
        AnimationQueue.Remove(path);

    public void DeQueuePriority(int priority)
    {
        if (AnimationQueue.FirstOrDefault(x => x.Key.Priority == priority) is {} animation)
            AnimationQueue.Remove(animation);
    }
    
    public Maybe<Animation> Animation(IPath path) => 
        _animationMap.TryGetValue(path.Path, out var a) ? a : Maybe<Animation>.None;

    public bool HasAnimation(IPath path) =>
        _animationMap.ContainsKey(path.Path);
    
    public bool IsCurrentAnimation(IPath path) =>
        Animation(path).Match(
            x => x == _currentAnimation,
            () =>
            {
                Log.Warning("Animation {Animation} is not stored", path);
                return false;
            });

    public bool IsCurrentAnimationLooping => _currentAnimation?.Loop ?? false;
    
    public bool IsCurrentAnimationFinished => _currentAnimation?.IsFinished() ?? true;


    public bool IsAnimationQueued(IPath requestedAnimation) => 
        AnimationQueue.Any(x => x.Key.Path == requestedAnimation.Path);
    
    private void LoadAnimationAssets(IPath path)
    {
        ResourceLoader.GetResourcesIn(path, "*.png")
            .ForEach(x => 
                _animationMap[x.Key] = Utils.Components.Draw.Animation.FromCollection(x.Files));
    }
    
}