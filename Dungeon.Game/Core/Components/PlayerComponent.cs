using CSharpFunctionalExtensions;

namespace Dungeon.Game.Core.Components;

public sealed class PlayerComponent : IComponent
{
    public record InputData(bool Repeate, Action<Entity> Callback, bool Pausable = false);

    private readonly Dictionary<int, InputData> _callback = new();
    private int _openDialogs = 0;
    
    public PlayerComponent()
    {
        
    }

    public IReadOnlyDictionary<int, InputData> Callbacks => _callback;

    
    public Maybe<Action<Entity>> RegisterCallback(
        int key,
        Action<Entity> callback,
        bool repeatable = true,
        bool pauseable = false)
    {
        _callback.TryGetValue(key, out var oldCallback);
        _callback[key] = new InputData(repeatable, callback, pauseable);
        return oldCallback?.Callback;
    }

    public void RemoveCallback(int key) => 
        _callback.Remove(key);
    
    public void RemoveCallbacks() => 
        _callback.Clear();
    
    
    public void IncrementOpenDialogs() =>
        _openDialogs++;
    
    public void DecrementOpenDialogs() =>
        _openDialogs--;
    
    public bool OpenDialogs() =>
        _openDialogs > 0;
    
}