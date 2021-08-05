using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Vertical.SpectreLogger.Options
{
    /// <summary>
    /// Defines a group of colors and settings that determine how segments are
    /// rendered.
    /// </summary>
    public class FormattingProfile
    {
        /// <summary>
        /// Gets the log level.
        /// </summary>
        public LogLevel LogLevel { get; internal set; }
        
        /// <summary>
        /// Gets or sets the output template.
        /// </summary>
        public string? OutputTemplate { get; set; }
        
        /// <summary>
        /// Gets or sets the base style markup to apply before any rendering.
        /// </summary>
        public string? BaseEventStyle { get; set; }

        /// <summary>
        /// Gets or sets style markup to apply before rendering a value of a specific type.
        /// The key is the type name.
        /// </summary>
        public Dictionary<Type, string> TypeStyles { get; } = new();
        
        /// <summary>
        /// Gets or sets the markup to apply before rendering a value where the type is
        /// not found in <see cref="TypeStyles"/>.
        /// </summary>
        public string? DefaultTypeStyle { get; set; }

        /// <summary>
        /// Gets or sets the markup to apply before rendering a specific value of a type. The
        /// key is a tuple combining the type and value.
        /// </summary>
        public Dictionary<(Type, object), string> ValueStyles { get; } = new();

        /// <summary>
        /// Gets or sets the name to display as the log level.
        /// </summary>
        public string LogLevelDisplay { get; set; } = "None";

        /// <summary>
        /// Gets a dictionary of functions that convert object value types
        /// to specific string representations.
        /// </summary>
        public Dictionary<Type, Func<object?, string?>> TypeFormatters { get; } = new();

        /// <summary>
        /// Gets or sets the formatting function to use if one cannot be
        /// found in <see cref="TypeFormatters"/>.
        /// </summary>
        public Func<object?, string?> DefaultTypeFormatter { get; } = obj => obj?.ToString();

        /// <summary>
        /// Gets a dictionary of option objects keyed by specific type.
        /// </summary>
        public Dictionary<Type, object> RendererOptions { get; } = new();

        /// <inheritdoc />
        public override string ToString() => LogLevel.ToString();
    }
}