using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Options;

namespace Vertical.SpectreLogger
{
    public class SpectreConsoleLoggerProvider : ILoggerProvider
    {
        private readonly ILogEventFilter _eventFilter;
        private readonly SpectreConsoleLoggerOptions _options;
        private readonly ConcurrentDictionary<string, ILogger> _cachedLoggers = new();

        public SpectreConsoleLoggerProvider(IOptions<SpectreConsoleLoggerOptions> optionsProvider,
            ILogEventFilter eventFilter)
        {
            _eventFilter = eventFilter;
            _options = optionsProvider.Value;
        }
        
        /// <inheritdoc />
        public void Dispose()
        {
        }

        /// <inheritdoc />
        public ILogger CreateLogger(string categoryName)
        {
            return _cachedLoggers.GetOrAdd(categoryName, id => new SpectreConsoleLogger(
                this, 
                _options, 
                _eventFilter,
                id));
        }

        internal object?[] Scopes => Array.Empty<object?>();
    }
}