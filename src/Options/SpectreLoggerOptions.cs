using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Vertical.SpectreLogger.Options
{
    /// <summary>
    /// Defines options for the logger.
    /// </summary>
    public class SpectreLoggerOptions
    {
        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        public SpectreLoggerOptions()
        {
            SpectreLoggerDefaults.ConfigureDefaults(this);    
        }
        
        /// <summary>
        /// Gets or sets the minimum log level for events to be output to the console.
        /// </summary>
        public LogLevel MinimumLevel { get; set; } = LogLevel.Information;

        /// <summary>
        /// Gets or sets the formatting profiles.
        /// </summary>
        public Dictionary<LogLevel, FormattingProfile> FormattingProfiles { get; } = new();
    }
}