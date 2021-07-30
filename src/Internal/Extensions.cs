using System;
using System.Collections.Generic;

namespace Vertical.SpectreLogger.Internal
{
    internal static class Extensions
    {
        internal static TValue? GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,
            TKey key,
            TValue? defaultIfNotFound = default)
        {
            return dictionary.TryGetValue(key, out var value)
                ? value
                : defaultIfNotFound;
        }

        internal static T Configure<T>(this T instance, Action<T> configure)
        {
            configure(instance);
            return instance;
        }
    }
}