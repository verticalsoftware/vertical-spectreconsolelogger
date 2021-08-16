using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Vertical.SpectreLogger.Core;

namespace Vertical.SpectreLogger.Options
{
    /// <summary>
    /// Represents options for <see cref="SpectreLogger"/>
    /// </summary>
    public partial class SpectreLoggerOptions
    {
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

        /// <summary>
        /// Gets a collection of types available to the rendering pipeline.
        /// </summary>
        public HashSet<Type> RendererTypes { get; } = new();
        
        /// <summary>
        /// Gets or sets an object that can dynamically control the log level.
        /// </summary>
        public ILogLevelController? LogLevelController { get; set; }
    }
}