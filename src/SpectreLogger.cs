using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Internal;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Output;

namespace Vertical.SpectreLogger
{
    public class SpectreLogger : ILogger
    {
        private readonly ILogEventFilter? _logEventFilter;
        private readonly IRendererPipeline _rendererPipeline;
        private readonly ScopeManager _scopeManager;
        private readonly SpectreLoggerOptions _options;

        internal SpectreLogger(
            IRendererPipeline rendererPipeline,
            SpectreLoggerOptions options,
            ScopeManager scopeManager)
        {
            _rendererPipeline = rendererPipeline;
            _scopeManager = scopeManager;
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
            
            var eventInfo = new LogEventContext(
                logLevel,
                eventId,
                state,
                exception,
                profile);

            if (_logEventFilter?.Filter(eventInfo) == true)
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