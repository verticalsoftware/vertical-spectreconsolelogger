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


        internal static void AddState<TState>(this Dictionary<string, object?> properties, TState state)
        {
            switch (state)
            {
                case IEnumerable<KeyValuePair<string, object>> keyValuePairs:
                    foreach (var entry in keyValuePairs)
                    {
                        properties[entry.Key] = entry.Value;
                    }

                    break;
                
                case KeyValuePair<string, object> keyValuePair:
                    properties[keyValuePair.Key] = keyValuePair.Value;
                    break;
            }
        }

        internal static void AddScopes(this Dictionary<string, object?> properties, IEnumerable<object?> scopes)
        {
            foreach (var obj in scopes)
            {
                properties.AddState(obj);
            }
        }
    }
}