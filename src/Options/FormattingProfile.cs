using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Vertical.SpectreLogger.Options
{
    /// <summary>
    /// Defines customizable options for a particular <see cref="LogLevel"/>
    /// </summary>
    public class FormattingProfile
    {
        internal FormattingProfile(LogLevel logLevel)
        {
            LogLevel = logLevel;
        }

        /// <summary>
        /// Gets the <see cref="LogLevel"/> for which the profile defines options for.
        /// </summary>
        public LogLevel LogLevel { get; }

        /// <summary>
        /// Gets a dictionary of functions that format specific types of values. The function receives
        /// the original value and returns the string representation to display.
        /// </summary>
        public Dictionary<Type, Func<object, string>> TypeFormatters { get; } = new();

        /// <summary>
        /// Gets a dictionary of functions that format specific values. The function receives the original
        /// value and returns the string representation to display.
        /// </summary>
        public Dictionary<object, Func<object, string>> ValueFormatters { get; } = new();

        /// <summary>
        /// Gets a dictionary of markup strings that are applied when rendering values of a specific
        /// type.
        /// </summary>
        public Dictionary<Type, string> TypeStyles { get; } = new();

        /// <summary>
        /// Gets a dictionary of markup strings that are applied when rendering specific values.
        /// </summary>
        public Dictionary<object, string> ValueStyles { get; } = new();
    }
}