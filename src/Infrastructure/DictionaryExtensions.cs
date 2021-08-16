
using System.Collections.Generic;

namespace Vertical.SpectreLogger.Infrastructure
{
    internal static class DictionaryExtensions
    {
        internal static void CopyTo<TKey, TValue>(this IDictionary<TKey, TValue> source,
            IDictionary<TKey, TValue> dest)
        {
            foreach (var entry in source)
            {
                dest.Add(entry.Key, entry.Value);
            }
        }

        internal static TValue? GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,
            TKey key,
            TValue? defaultValue = default)
        {
            return dictionary.TryGetValue(key, out var value)
                ? value
                : defaultValue;
        }
    }
}