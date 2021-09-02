using System;
using Microsoft.Extensions.Logging;
using Vertical.SpectreLogger.Options;

namespace Vertical.SpectreLogger.Core
{
    public readonly struct LogEventContext
    {
        internal LogEventContext(
            LogLevel logLevel,
            EventId eventId,
            object? state,
            Exception exception,
            LogLevelProfile profile)
        {
            LogLevel = logLevel;
            EventId = eventId;
            State = state;
            Exception = exception;
            Profile = profile;
        }

        /// <summary>
        /// Gets the log level.
        /// </summary>
        public LogLevel LogLevel { get; }

        /// <summary>
        /// Gets the event id.
        /// </summary>
        public EventId EventId { get; }
        
        /// <summary>
        /// Gets the event state data.
        /// </summary>
        public object? State { get; }
        
        /// <summary>
        /// Gets the exception.
        /// </summary>
        public Exception Exception { get; }
        
        /// <summary>
        /// Gets the log level profile.
        /// </summary>
        public LogLevelProfile Profile { get; }
    }
}