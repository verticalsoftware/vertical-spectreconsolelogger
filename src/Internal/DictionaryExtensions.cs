using System.Collections.Generic;

namespace Vertical.SpectreLogger.Internal
{
    internal static class DictionaryExtensions
    {
        internal static TValue? GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,
            TKey key,
            TValue? defaultIfNotFound = default)
        {
            return dictionary.TryGetValue(key, out var value)
                ? value
                : defaultIfNotFound;
        }
    }
}