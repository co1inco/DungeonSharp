namespace Dungeon.Game.Core.Utils.Components.Draw;

public sealed class Animation
{
    public const string MISSING_TEXTURE = "animation/missing_texture.png";
    public const int DEFAULT_FRAME_TIME = 5;
    public const bool DEFAULT_IS_LOOP = true;
    public const int DEFAULT_PRIORITY = 200;

    private readonly List<string> _animationFrames;
    private int _currentFrameIndex;
    private int _frameTimeCounter;


    private Animation(IEnumerable<string> animationFrames, int frameTimes, bool looping, int priority)
    {
        _animationFrames = animationFrames.ToList();
        if (!_animationFrames.Any())
            throw new ArgumentException("No animation frames provided.");
        
        ArgumentOutOfRangeException.ThrowIfNegative(frameTimes);
        TimeBetweenFrames = frameTimes;
        Loop = looping;
        Priority = priority;
    }

    public static Animation FromCollection(
        IEnumerable<string> animationFrames,
        int frameTimes = DEFAULT_FRAME_TIME,
        bool looping = DEFAULT_IS_LOOP) =>
        new(animationFrames, frameTimes, looping, CoreAnimationProperties.DEFAULT.Priority);
    
    public static Animation FromCollection(
        IEnumerable<string> animationFrames,
        int priority,
        int frameTimes = DEFAULT_FRAME_TIME,
        bool looping = DEFAULT_IS_LOOP) =>
        new(animationFrames, frameTimes, looping, priority);
    
    public static Animation FromSingleImage(string fileName, int frameTimes = DEFAULT_FRAME_TIME) =>
        new Animation([fileName], frameTimes, DEFAULT_IS_LOOP, DEFAULT_PRIORITY);
    
    public static Animation DefaultAnimation() =>
        new Animation([MISSING_TEXTURE], DEFAULT_FRAME_TIME, DEFAULT_IS_LOOP, 0);




    public IReadOnlyCollection<string> AnimationFrames => _animationFrames;
    
    public bool Loop { get; set; }

    public int TimeBetweenFrames { get; set; }

    
    public int Priority { get; }

    public int Duration => TimeBetweenFrames * _animationFrames.Count;

    public string NextAnimationTexturePath()
    {
        // TODO: split tasks of this function
        //This "also" advances the frame counter by one. should be separated into GetAnimationFrame and Advance or something
        // This method returns the CURRENT texture and advances the frameTimeCounter
        if (IsFinished())
            return _animationFrames[_currentFrameIndex];
        
        var path = _animationFrames[_currentFrameIndex];
        _frameTimeCounter = (_frameTimeCounter + 1) % TimeBetweenFrames;
        if (_frameTimeCounter == 0)
            _currentFrameIndex = (_currentFrameIndex + 1) % _animationFrames.Count;
        return path;
    }

    public bool IsFinished() => 
        !Loop && _currentFrameIndex == _animationFrames.Count - 1;
    
    
}