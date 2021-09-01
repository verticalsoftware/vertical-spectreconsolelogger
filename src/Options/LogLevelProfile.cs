using Microsoft.Extensions.Logging;

namespace Vertical.SpectreLogger.Options
{
    /// <summary>
    /// Defines options to be applied to a specific log level.
    /// </summary>
    public class LogLevelProfile
    {
        internal LogLevelProfile(LogLevel logLevel)
        {
            LogLevel = logLevel;
        }

        /// <summary>
        /// Gets the log level.
        /// </summary>
        public LogLevel LogLevel { get; }

        /// <summary>
        /// Gets or sets the output template.
        /// </summary>
        public string? OutputTemplate { get; set; } = default!;
    }
}