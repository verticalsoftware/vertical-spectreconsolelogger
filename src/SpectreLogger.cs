using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Vertical.SpectreLogger.Internal;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.Rendering;
using Vertical.SpectreLogger.Rendering.Internal;

namespace Vertical.SpectreLogger
{
    public class SpectreLogger : ILogger
    {
        private readonly SpectreLoggerProvider _provider;
        private readonly SpectreLoggerOptions _options;
        private readonly IWriteBufferFactory _writeBufferFactory;
        private readonly string _categoryName;
        private readonly ITemplateRendererBuilder _rendererBuilder;

        public SpectreLogger(SpectreLoggerProvider provider,
            SpectreLoggerOptions options, 
            IWriteBufferFactory writeBufferFactory,
            ITemplateRendererBuilder rendererBuilder,
            string categoryName)
        {
            _provider = provider;
            _options = options;
            _writeBufferFactory = writeBufferFactory;
            _categoryName = categoryName;
            _rendererBuilder = rendererBuilder;
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
            
            var properties = new Dictionary<string, object?>();
            
            properties.AddState(state);
            properties.AddScopes(_provider.Scopes);

            var eventInfo = new LogEventInfo(_categoryName, 
                logLevel, 
                eventId, 
                state, 
                exception,
                properties,
                _provider.Scopes,
                _options.FormattingProfiles[logLevel]);

            if (IsFiltered(eventInfo))
                return;

            using var buffer = _writeBufferFactory.GetInstance();
            

            var renderers = _rendererBuilder.GetRenderers(logLevel);
            var length = renderers.Length;

            for (var c = 0; c < length; c++)
            {
                renderers[c].Render(buffer, eventInfo);
            }
            
            buffer.Flush();
        }

        private bool IsFiltered(in LogEventInfo eventInfo)
        {
            var length = _options.Filters.Count;
            var filters = _options.Filters;

            for (var c = 0; c < length; c++)
            {
                if (filters[c](eventInfo))
                    return true;
            }

            return false;
        }

        /// <inheritdoc />
        public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None && logLevel >= _options.MinimumLevel;

        /// <inheritdoc />
        public IDisposable BeginScope<TState>(TState state) => _provider.BeginLoggerScope(state);
    }
}