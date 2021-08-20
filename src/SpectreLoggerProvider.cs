using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Infrastructure;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Output;

namespace Vertical.SpectreLogger
{
    internal class SpectreLoggerProvider : ILoggerProvider
    {
        private readonly IWriteBufferFactory _bufferFactory;
        private readonly ILogLevelController _logLevelController;
        private readonly IScopeManager _scopeManager;
        private readonly ConcurrentDictionary<string, ILogger> _cachedLoggers = new();
        private readonly Dictionary<LogLevel,RuntimeFormattingProfile> _runtimeProfiles;

        public SpectreLoggerProvider(
            IOptions<SpectreLoggerOptions> optionsProvider,
            ITemplateRendererFactory templateRendererFactory,
            IWriteBufferFactory bufferFactory,
            ILogLevelController logLevelController,
            IScopeManager scopeManager)
        {
            _bufferFactory = bufferFactory;
            _logLevelController = logLevelController;
            _scopeManager = scopeManager;
            _runtimeProfiles = BuildRuntimeProfiles(optionsProvider.Value, templateRendererFactory);
        }
        
        /// <inheritdoc />
        public void Dispose()
        {
        }

        /// <inheritdoc />
        public ILogger CreateLogger(string categoryName)
        {
            return _cachedLoggers.GetOrAdd(categoryName, id => new SpectreLogger(
                _runtimeProfiles,
                _bufferFactory,
                _scopeManager,
                _logLevelController,
                id));
        }

        private static Dictionary<LogLevel, RuntimeFormattingProfile> BuildRuntimeProfiles(SpectreLoggerOptions options,
            ITemplateRendererFactory rendererFactory)
        {
            return options
                .FormattingProfiles
                .Values
                .Select(profile => new RuntimeFormattingProfile(profile, rendererFactory.CreatePipeline(profile)))
                .ToDictionary(profile => profile.LogLevel);
        }
    }
}