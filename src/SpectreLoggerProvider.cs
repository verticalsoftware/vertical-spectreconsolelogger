using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Internal;
using Vertical.SpectreLogger.Options;

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
        
        private readonly ConcurrentDictionary<string, ILogger> _cachedLoggers = new();
        
        /// <summary>
        /// Creates a new instance of this provider type.
        /// </summary>
        public SpectreLoggerProvider(
            IOptions<SpectreLoggerOptions> optionsProvider,
            IRendererPipeline rendererPipeline)
        {
            _optionsProvider = optionsProvider;
            _rendererPipeline = rendererPipeline;
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
                _scopeManager,
                name));
        }
    }
}