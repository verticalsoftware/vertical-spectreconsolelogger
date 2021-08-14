using System;
using Microsoft.Extensions.Logging;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Infrastructure;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Output;

namespace Vertical.SpectreLogger
{
    internal class SpectreLogger : ILogger
    {
        private readonly IConfiguredLoggingContext _loggingContext;
        private readonly IWriteBufferProvider _bufferProvider;
        private readonly IScopeManager _scopeManager;
        private readonly string _categoryName;

        internal SpectreLogger(
            IConfiguredLoggingContext loggingContext,
            IWriteBufferProvider bufferProvider,
            IScopeManager scopeManager,
            string categoryName)
        {
            _loggingContext = loggingContext;
            _bufferProvider = bufferProvider;
            _scopeManager = scopeManager;
            _categoryName = categoryName;
        }
        
        /// <inheritdoc />
        public void Log<TState>(LogLevel logLevel, 
            EventId eventId, 
            TState state, 
            Exception exception, 
            Func<TState, Exception, string> formatter)
        {
            var formattingProfile = _loggingContext.GetFormattingProfile(logLevel);
            var scopes = _scopeManager.Scopes;
            
            var eventInfo = new LogEventInfo(
                _categoryName,
                logLevel,
                eventId,
                state,
                scopes,
                exception,
                formattingProfile);

            if (!formattingProfile.LogEventFilter?.Invoke(eventInfo) ?? true)
                return;

            using var buffer = _bufferProvider.GetInstance();
            var pipeline = _loggingContext.GetRenderingPipeline(logLevel);

            foreach (var renderer in pipeline)
            {
                renderer.Render(buffer, eventInfo);
            }
        }

        /// <inheritdoc />
        public bool IsEnabled(LogLevel logLevel) => logLevel >= _loggingContext.MinimumLevel;

        /// <inheritdoc />
        public IDisposable BeginScope<TState>(TState state) => _scopeManager.BeginScope(state);
    }
}