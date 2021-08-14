using System;
using Microsoft.Extensions.Logging;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Options;

namespace Vertical.SpectreLogger
{
    public class SpectreLogger : ILogger
    {
        private readonly SpectreLoggerProvider _provider;
        private readonly SpectreConsoleLoggerOptions _options;
        private readonly ILogEventFilter _eventFilter;
        private readonly string _categoryName;

        public SpectreLogger(SpectreLoggerProvider provider,
            SpectreConsoleLoggerOptions options,
            ILogEventFilter eventFilter,
            string categoryName)
        {
            _provider = provider;
            _options = options;
            _eventFilter = eventFilter;
            _categoryName = categoryName;
        }
        
        /// <inheritdoc />
        public void Log<TState>(LogLevel logLevel, 
            EventId eventId, 
            TState state, 
            Exception exception, 
            Func<TState, Exception, string> formatter)
        {
            var scopes = _provider.Scopes;
            var eventInfo = new LogEventInfo(
                _categoryName,
                logLevel,
                eventId,
                state,
                scopes,
                exception,
                null!);

            if (!_eventFilter.Render(eventInfo))
                return;
            
            
        }

        /// <inheritdoc />
        public bool IsEnabled(LogLevel logLevel) => logLevel >= _options.MinimumLevel && logLevel > LogLevel.None;

        /// <inheritdoc />
        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }
    }
}