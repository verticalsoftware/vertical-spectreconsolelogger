using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Vertical.SpectreLogger.Infrastructure;
using Vertical.SpectreLogger.Output;

namespace Vertical.SpectreLogger
{
    public class SpectreLoggerProvider : ILoggerProvider
    {
        private readonly IConfiguredLoggingContext _loggingContext;
        private readonly IWriteBufferProvider _bufferProvider;
        private readonly IScopeManager _scopeManager;
        private readonly ConcurrentDictionary<string, ILogger> _cachedLoggers = new();

        public SpectreLoggerProvider(IConfiguredLoggingContext loggingContext,
            IWriteBufferProvider bufferProvider,
            IScopeManager scopeManager)
        {
            _loggingContext = loggingContext;
            _bufferProvider = bufferProvider;
            _scopeManager = scopeManager;
        }
        
        /// <inheritdoc />
        public void Dispose()
        {
        }

        /// <inheritdoc />
        public ILogger CreateLogger(string categoryName)
        {
            return _cachedLoggers.GetOrAdd(categoryName, id => new SpectreLogger(
                _loggingContext,
                _bufferProvider,
                _scopeManager,
                id));
        }
    }
}