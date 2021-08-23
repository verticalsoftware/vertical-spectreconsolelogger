using System.Collections.Generic;
using System.Linq;

namespace Vertical.SpectreLogger.Utilities
{
    public static class PropertyExtensions
    {
        public static bool TryGetFormattedLogValue(
            this IReadOnlyList<KeyValuePair<string, object>> formattedLogValues,
            string key, out object? value)
        {
            var keyValuePair = formattedLogValues.FirstOrDefault(kv => kv.Key == key);
            
            value = keyValuePair.Value;

            return keyValuePair.Key != null;
        }
        
        public static bool TryGetFormattedLogValue<T>(
            this IReadOnlyList<KeyValuePair<string, object>> formattedLogValues,
            string key, out T? value)
        {
            var keyValuePair = formattedLogValues.FirstOrDefault(kv => kv.Key == key);

            if (keyValuePair.Key == null)
            {
                value = default;
                return false;
            }

            try
            {
                value = (T) keyValuePair.Value;
                return true;
            }
            catch
            {
                value = default;
                return false;
            }
        }
    }
}