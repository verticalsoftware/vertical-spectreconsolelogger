using System;
using Microsoft.Extensions.Logging;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.Rendering;

namespace Vertical.SpectreLogger
{
    public class SpectreLogger : ILogger
    {
        private readonly SpectreLoggerProvider _provider;
        private readonly SpectreLoggerOptions _options;
        private readonly IWriteBufferFactory _writeBufferFactory;
        private readonly string _categoryName;
        private readonly ITemplateRendererProvider _rendererProvider;

        public SpectreLogger(SpectreLoggerProvider provider,
            SpectreLoggerOptions options, 
            IWriteBufferFactory writeBufferFactory,
            ITemplateRendererProvider rendererProvider,
            string categoryName)
        {
            _provider = provider;
            _options = options;
            _writeBufferFactory = writeBufferFactory;
            _categoryName = categoryName;
            _rendererProvider = rendererProvider;
        }
        
        /// <inheritdoc />
        public void Log<TState>(LogLevel logLevel, 
            EventId eventId, 
            TState state, 
            Exception exception, 
            Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;

            var buffer = _writeBufferFactory.GetInstance();
            var eventInfo = new LogEventInfo(_categoryName, 
                logLevel, eventId, state, exception,
                _provider.Scopes,
                _options.FormattingProfiles[logLevel]);
            
            foreach (var templateFormatter in _rendererProvider.GetRenderers(logLevel))
            {
                templateFormatter.Format(buffer, ref eventInfo);
            }
            
            buffer.Flush();
        }
        
        /// <inheritdoc />
        public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None && logLevel >= _options.MinimumLevel;

        /// <inheritdoc />
        public IDisposable BeginScope<TState>(TState state) => _provider.BeginLoggerScope(state);
    }
}