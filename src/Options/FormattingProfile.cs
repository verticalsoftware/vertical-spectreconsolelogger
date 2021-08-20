using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Infrastructure;

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
        /// Gets or sets the output template.
        /// </summary>
        public string OutputTemplate { get; set; } = default!;
        
        /// <summary>
        /// Gets the style that is applied values not specifically registered in
        /// <see cref="ValueStyles"/> or <see cref="TypeStyles"/>
        /// </summary>
        public string? DefaultStyle { get; set; }
        
        /// <summary>
        /// Gets the formatter that is applied to values or types not specifically found
        /// in <see cref="ValueFormatters"/> or <see cref="TypeFormatters"/>
        /// </summary>
        public Func<object, string>? DefaultFormatter { get; set; }

        /// <summary>
        /// Gets a dictionary of functions that format specific types of values. The function receives
        /// the original value and returns the string representation to display.
        /// </summary>
        public Dictionary<Type, Func<object, string>> TypeFormatters { get; } = new();

        /// <summary>
        /// Gets a dictionary of functions that format specific values. The function receives the original
        /// value and returns the string representation to display.
        /// </summary>
        public Dictionary<object, string> ValueFormatters { get; } = new();

        /// <summary>
        /// Gets a dictionary of markup strings that are applied when rendering values of a specific
        /// type.
        /// </summary>
        public Dictionary<Type, string> TypeStyles { get; } = new();

        /// <summary>
        /// Gets a dictionary of markup strings that are applied when rendering specific values.
        /// </summary>
        public Dictionary<object, string> ValueStyles { get; } = new();
        
        /// <summary>
        /// Gets or sets a filter for each log event.
        /// </summary>
        public LogEventPredicate? LogEventFilter { get; set; }

        internal void CopyTo(FormattingProfile formattingProfile)
        {
            TypeFormatters.CopyTo(formattingProfile.TypeFormatters);
            TypeStyles.CopyTo(formattingProfile.TypeStyles);
            ValueFormatters.CopyTo(formattingProfile.ValueFormatters);
            ValueStyles.CopyTo(formattingProfile.ValueStyles);
            formattingProfile.LogEventFilter = LogEventFilter;
            formattingProfile.OutputTemplate = OutputTemplate;
        }
    }
}