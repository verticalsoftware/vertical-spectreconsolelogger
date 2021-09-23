using System;
using Microsoft.Extensions.Logging;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Internal;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Scopes;

namespace Vertical.SpectreLogger
{
    /// <summary>
    /// Implementation of the logger.
    /// </summary>
    public class SpectreLogger : ILogger
    {
        private readonly ILogEventFilter? _logEventFilter;
        private readonly IRendererPipeline _rendererPipeline;
        private readonly ScopeManager _scopeManager;
        private readonly string _categoryName;
        private readonly SpectreLoggerOptions _options;

        internal SpectreLogger(
            IRendererPipeline rendererPipeline,
            SpectreLoggerOptions options,
            ScopeManager scopeManager,
            string categoryName)
        {
            _rendererPipeline = rendererPipeline;
            _scopeManager = scopeManager;
            _categoryName = categoryName;
            _options = options;
            _logEventFilter = _options.LogEventFilter;
        }
        
        /// <inheritdoc />
        public void Log<TState>(
            LogLevel logLevel, 
            EventId eventId, 
            TState state, 
            Exception exception, 
            Func<TState, Exception, string> formatter)
        {
            if (ReferenceEquals(null, state))
            {
                // Nothing to render?
                return;
            }

            var profile = _options.LogLevelProfiles[logLevel];
            var scopeValues = _scopeManager.GetValues();
            
            var eventInfo = new LogEventContext(
                _categoryName,
                logLevel,
                eventId,
                state,
                exception,
                scopeValues,
                profile);

            if (!(_logEventFilter?.Filter(eventInfo)).GetValueOrDefault(true))
                return;
            
            _rendererPipeline.Render(eventInfo);
        }

        /// <inheritdoc />
        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel > LogLevel.None && logLevel >= _options.MinimumLogLevel;
        }

        /// <inheritdoc />
        public IDisposable BeginScope<TState>(TState state)
        {
            return _scopeManager.BeginScope(state);
        }
    }
}