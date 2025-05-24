namespace Dungeon.Game.Helper;

public static class DictionaryExtensions
{

    public static TValue ComputeIfAbsent<TKey, TValue>(this Dictionary<TKey, TValue> dictionary,
        TKey key,
        Func<TKey, TValue> valueGenerator) where TKey : notnull
    {
        if (key is null)
            return default(TValue);
        
        if (dictionary.TryGetValue(key, out var value) && value is not null)
            return value;
        
        value = valueGenerator(key);
        dictionary[key] = value;
        return value;
    }
    
}