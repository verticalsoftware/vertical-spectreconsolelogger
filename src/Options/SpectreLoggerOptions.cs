using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Vertical.SpectreLogger.Options
{
    /// <summary>
    /// Defines options for the logger.
    /// </summary>
    public partial class SpectreLoggerOptions
    {
        internal static readonly SpectreLoggerOptions Default = new();
        
        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        public SpectreLoggerOptions()
        {
            ConfigureDefaults(this);    
        }
        
        /// <summary>
        /// Gets or sets the minimum log level for events to be output to the console.
        /// </summary>
        public LogLevel MinimumLevel { get; set; } = LogLevel.Information;

        /// <summary>
        /// Gets or sets the formatting profiles.
        /// </summary>
        public IReadOnlyDictionary<LogLevel, FormattingProfile> FormattingProfiles { get; } =
            new ReadOnlyDictionary<LogLevel, FormattingProfile>(new[]
            {
                LogLevel.Trace,
                LogLevel.Debug,
                LogLevel.Information,
                LogLevel.Warning,
                LogLevel.Error,
                LogLevel.Critical
            }.ToDictionary(level => level, _ => new FormattingProfile()));

        /// <summary>
        /// Gets or sets a dictionary of log level filters.
        /// </summary>
        public List<LogEventFilter> Filters { get; } = new();

        /// <summary>
        /// Gets or sets the maximum number of write buffers to keep pooled for log events.
        /// </summary>
        public int MaxPooledWriteBuffers { get; set; } = 5;
    }
}