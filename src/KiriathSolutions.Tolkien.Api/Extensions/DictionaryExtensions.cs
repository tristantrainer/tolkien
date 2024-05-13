namespace KiriathSolutions.Tolkien.Api.Extensions;

internal static class DictionaryExtensions
{
    public static V GetOrUpdate<K, V>(this Dictionary<K, V> dictionary, K key, Func<V> updateValueFunction) where K : notnull
    {
        if (dictionary.ContainsKey(key))
            return dictionary[key];

        var value = updateValueFunction();
        dictionary[key] = value;
        return value;
    }
}
