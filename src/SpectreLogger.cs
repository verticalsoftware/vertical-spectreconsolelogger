using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Infrastructure;
using Vertical.SpectreLogger.Output;

namespace Vertical.SpectreLogger
{
    internal class SpectreLogger : ILogger
    {
        private readonly Dictionary<LogLevel, RuntimeFormattingProfile> _runtimeProfiles;
        private readonly IWriteBufferFactory _bufferFactory;
        private readonly IScopeManager _scopeManager;
        private readonly ILogLevelController _logLevelController;
        private readonly string _categoryName;

        internal SpectreLogger(
            Dictionary<LogLevel, RuntimeFormattingProfile> runtimeProfiles,
            IWriteBufferFactory bufferFactory,
            IScopeManager scopeManager,
            ILogLevelController logLevelController,
            string categoryName)
        {
            _runtimeProfiles = runtimeProfiles;
            _bufferFactory = bufferFactory;
            _scopeManager = scopeManager;
            _logLevelController = logLevelController;
            _categoryName = categoryName;
        }
        
        /// <inheritdoc />
        public void Log<TState>(LogLevel logLevel, 
            EventId eventId, 
            TState state, 
            Exception exception, 
            Func<TState, Exception, string> formatter)
        {
            var formattingProfile = _runtimeProfiles[logLevel];
            var scopes = _scopeManager.Scopes;
            
            var eventInfo = new LogEventInfo(
                _categoryName,
                logLevel,
                eventId,
                state,
                scopes,
                exception,
                formattingProfile);

            if (formattingProfile.LogEventFilter?.Invoke(eventInfo) == false)
                return;

            using var buffer = _bufferFactory.GetInstance();
            var pipeline = formattingProfile.RendererPipeline;
            var length = pipeline.Count;

            for (var c = 0; c < length; c++)
            {
                pipeline[c].Render(buffer, eventInfo);
            }
            
            buffer.Flush();
        }

        /// <inheritdoc />
        public bool IsEnabled(LogLevel logLevel) => logLevel > LogLevel.None && logLevel >= _logLevelController.MinimumLevel;

        /// <inheritdoc />
        public IDisposable BeginScope<TState>(TState state) => _scopeManager.BeginScope(state);
    }
}