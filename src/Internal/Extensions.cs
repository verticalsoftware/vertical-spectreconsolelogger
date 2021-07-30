using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

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

        internal static IDictionary<string, object?> AsFormattedLogValues<TState>(this TState state) =>
            (state as IEnumerable<KeyValuePair<string, object>>)?.ToDictionary(kv => kv.Key, kv => (object?) kv.Value)
            ??
            (IDictionary<string, object?>) ImmutableDictionary<string, object?>.Empty;
    }
}