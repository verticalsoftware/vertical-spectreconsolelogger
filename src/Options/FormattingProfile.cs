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
        /// Gets a dictionary of option objects keyed by specific type.
        /// </summary>
        public Dictionary<Type, object> RendererOptions { get; } = new();

        /// <inheritdoc />
        public override string ToString() => LogLevel.ToString();
    }
}