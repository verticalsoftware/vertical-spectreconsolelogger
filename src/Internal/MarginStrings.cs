using System;
using System.Collections.Generic;

namespace Vertical.SpectreLogger.Internal
{
    internal sealed class MarginStrings
    {
        internal static readonly MarginStrings Instance = new MarginStrings();
        
        private MarginStrings()
        {
        }

        private readonly object _theLock = new object();
        private readonly Dictionary<int, string> _cachedStrings = new();

        /// <summary>
        /// Gets a margin string for the specified number of spaces.
        /// </summary>
        /// <param name="count">Number of spaces.</param>
        public string this[int count]
        {
            get
            {
                if (count < 0)
                {
                    throw new ArgumentException("Count cannot be less than zero.");
                }

                if (count == 0)
                {
                    return string.Empty;
                }
                
                lock (_theLock)
                {
                    if (_cachedStrings.TryGetValue(count, out var str))
                        return str;

                    var entry = new string(' ', count);
                    
                    _cachedStrings.Add(count, entry);

                    return entry;
                }
            }
        }
    }
}