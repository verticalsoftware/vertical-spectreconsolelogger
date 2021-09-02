using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using Spectre.Console;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Internal;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Output;

namespace Vertical.SpectreLogger
{
    /// <summary>
    /// Logger provider for <see cref="SpectreLogger"/>
    /// </summary>
    public class SpectreLoggerProvider : ILoggerProvider
    {
        private readonly IOptions<SpectreLoggerOptions> _optionsProvider;
        private readonly IRendererPipeline _rendererPipeline;
        private readonly ScopeManager _scopeManager = new();
        private readonly DefaultObjectPool<IWriteBuffer> _writeBufferPool;
        private readonly ConcurrentDictionary<string, ILogger> _cachedLoggers = new();
        
        /// <summary>
        /// Creates a new instance of this provider type.
        /// </summary>
        public SpectreLoggerProvider(
            IOptions<SpectreLoggerOptions> optionsProvider,
            IRendererPipeline rendererPipeline,
            IConsoleWriter consoleWriter)
        {
            _optionsProvider = optionsProvider;
            _rendererPipeline = rendererPipeline;
            _writeBufferPool = new(new WriteBufferPooledObjectPolicy(consoleWriter),
                5);
        }
        
        /// <inheritdoc />
        public void Dispose()
        {
            // Not implemented
        }

        /// <inheritdoc />
        public ILogger CreateLogger(string categoryName)
        {
            return _cachedLoggers.GetOrAdd(categoryName, name => new SpectreLogger(
                _rendererPipeline,
                _optionsProvider.Value,
                _writeBufferPool,
                _scopeManager));
        }
    }
}