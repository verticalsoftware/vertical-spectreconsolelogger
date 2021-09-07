using System;
using System.Collections.Generic;

namespace Vertical.SpectreLogger.Reflection
{
    internal static class DestructuringHelpers
    {
        /// <summary>
        /// Determines whether the given type is a dictionary.
        /// </summary>
        /// <param name="type">Type to evaluate.</param>
        /// <returns>Boolean</returns>
        internal static bool IsDictionary(Type type)
        {
            return type.IsGenericType && typeof(IDictionary<,>).IsAssignableFrom(type.GetGenericTypeDefinition());
        }

        /// <summary>
        /// Gets the key/value pairs of a dictionary of unknown type.
        /// </summary>
        /// <param name="dictionary">Dictionary instance.</param>
        /// <param name="entryCallback">Delegate that receives each item.</param>
        internal static void GetDictionaryEntries(
            object dictionary,
            Action<string, object> entryCallback)
        {
            
        }
    }
}