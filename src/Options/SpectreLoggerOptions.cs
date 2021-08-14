using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Vertical.SpectreLogger.Core;

namespace Vertical.SpectreLogger.Options
{
    /// <summary>
    /// Represents options for <see cref="SpectreLogger"/>
    /// </summary>
    public class SpectreConsoleLoggerOptions
    {
        /// <summary>
        /// Gets or sets the minimum log level.
        /// </summary>
        public LogLevel MinimumLevel { get; set; } = LogLevel.Trace;
        
        /// <summary>
        /// Gets or sets a function used to filter events. The predicate should return a
        /// boolean value indicating whether the event should be rendered.
        /// </summary>
        public LogEventPredicate? EventFilter { get; set; }

        /// <summary>
        /// Gets a dictionary of <see cref="FormattingProfiles"/>, one for each <see cref="LogLevel"/>
        /// value with the exception of <see cref="LogLevel.None"/>
        /// </summary>
        public Dictionary<LogLevel, FormattingProfile> FormattingProfiles { get; } = new()
        {
            [LogLevel.Trace] = new FormattingProfile(LogLevel.Trace),
            [LogLevel.Debug] = new FormattingProfile(LogLevel.Debug),
            [LogLevel.Information] = new FormattingProfile(LogLevel.Information),
            [LogLevel.Warning] = new FormattingProfile(LogLevel.Warning),
            [LogLevel.Error] = new FormattingProfile(LogLevel.Error),
            [LogLevel.Critical] = new FormattingProfile(LogLevel.Critical)
        };
    }
}