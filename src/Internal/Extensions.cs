using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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
                
                case IEnumerable<ValueTuple<string, object>> valueTuples:
                    foreach (var (key, value) in valueTuples)
                    {
                        properties[key] = value;
                    }
                    break;
                
                case IEnumerable<Tuple<string, object>> tuples:
                    foreach (var (key, value) in tuples)
                    {
                        properties[key] = value;
                    }
                    break;
                
                case KeyValuePair<string, object> keyValuePair:
                    properties[keyValuePair.Key] = keyValuePair.Value;
                    break;
                
                case ValueTuple<string, object>(var key, var value):
                    properties[key] = value;
                    break;
                
                case Tuple<string, object>(var key, var value):
                    properties[key] = value;
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