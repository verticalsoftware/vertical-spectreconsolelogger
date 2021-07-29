using System;
using System.Collections.Generic;

namespace Vertical.SpectreLogger.Options
{
    /// <summary>
    /// Defines a group of colors and settings that determine how segments are
    /// rendered.
    /// </summary>
    public class FormattingProfile
    {
        /// <summary>
        /// Gets or sets the output template.
        /// </summary>
        public string? OutputTemplate { get; set; }
        
        /// <summary>
        /// Gets or sets the base style markup to apply before any rendering.
        /// </summary>
        public string? BaseMarkup { get; set; }
        
        /// <summary>
        /// Gets or sets the style markup to apply before rendering the log level.
        /// </summary>
        public string? LogLevelMarkup { get; set; }

        /// <summary>
        /// Gets or sets style markup to apply before rendering a value of a specific type.
        /// The key is the type name.
        /// </summary>
        public Dictionary<Type, string> TypeStyles { get; } = new();

        /// <summary>
        /// Gets or sets the name to display as the log level.
        /// </summary>
        public string LogLevelDisplay { get; set; } = "None";

        /// <summary>
        /// Gets a dictionary of functions that convert object value types
        /// to specific string representations.
        /// </summary>
        public Dictionary<Type, Func<object?, string?>> ValueFormatters { get; } = new();
        
        /// <summary>
        /// Gets or sets the number of spaces to indent new lines in logging output.
        /// </summary>
        public int NewLineIndent { get; set; }

        /// <summary>
        /// Gets a dictionary of option objects keyed by specific type.
        /// </summary>
        public Dictionary<Type, object> RendererOptions { get; } = new();
    }
}