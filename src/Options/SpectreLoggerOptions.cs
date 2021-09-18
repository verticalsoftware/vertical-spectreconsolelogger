using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Internal;

namespace Vertical.SpectreLogger.Options
{
    public class SpectreLoggerOptions
    {
        private int _maxPooledBuffers = 5;
        
        /// <summary>
        /// Gets or sets the minimum log level.
        /// </summary>
        public LogLevel MinimumLogLevel { get; set; } = LogLevel.Information;
        
        /// <summary>
        /// Gets or sets an object that controls log event filtering.
        /// </summary>
        public ILogEventFilter? LogEventFilter { get; set; }
        
        /// <summary>
        /// Gets the log level profiles.
        /// </summary>
        public IReadOnlyDictionary<LogLevel, LogLevelProfile> LogLevelProfiles { get; } =
            new Dictionary<LogLevel, LogLevelProfile>
            {
                [LogLevel.Trace] = new(LogLevel.Trace),
                [LogLevel.Debug] = new(LogLevel.Debug),
                [LogLevel.Information] = new(LogLevel.Information),
                [LogLevel.Warning] = new(LogLevel.Warning),
                [LogLevel.Error] = new(LogLevel.Error),
                [LogLevel.Critical] = new(LogLevel.Critical)
            };

        /// <summary>
        /// Gets or sets the maximum number of pooled buffers.
        /// </summary>
        public int MaxPooledBuffers
        {
            get => _maxPooledBuffers;
            set => _maxPooledBuffers = Math.Max(1, value);
        }
    }
}