using System;
using Microsoft.Extensions.Logging;
using Vertical.SpectreLogger.Options;

namespace Vertical.SpectreLogger.Core
{
    public readonly struct LogEventInfo
    {
        internal LogEventInfo(
            LogLevel logLevel,
            EventId eventId,
            object state,
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

        public EventId EventId { get; }
        public object State { get; }
        public Exception Exception { get; }
        public LogLevelProfile Profile { get; }
    }
}