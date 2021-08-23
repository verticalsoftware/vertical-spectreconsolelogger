using System;
using Microsoft.Extensions.Logging;
using Vertical.SpectreLogger.Options;

namespace Vertical.SpectreLogger.Core
{
    /// <summary>
    /// A structure that defines the data of a log event.
    /// </summary>
    public readonly struct LogEventInfo
    {
        public LogEventInfo(
            string categoryName,
            LogLevel logLevel,
            EventId eventId,
            object? state,
            object?[] scopes,
            Exception? exception,
            FormattingProfile formattingProfile)
        {
            CategoryName = categoryName;
            LogLevel = logLevel;
            EventId = eventId;
            State = state;
            Scopes = scopes;
            Exception = exception;
            FormattingProfile = formattingProfile;
        }

        public string CategoryName { get; }
        public LogLevel LogLevel { get; }

        public EventId EventId { get; }

        public object? State { get; }

        public object?[] Scopes { get; }

        public Exception? Exception { get; }

        public FormattingProfile FormattingProfile { get; }
    }
}