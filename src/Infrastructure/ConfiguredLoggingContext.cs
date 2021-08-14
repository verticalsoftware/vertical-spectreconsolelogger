using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Options;

namespace Vertical.SpectreLogger.Infrastructure
{
    internal class ConfiguredLoggingContext : IConfiguredLoggingContext
    {
        private readonly SpectreLoggerOptions _options;

        public ConfiguredLoggingContext(IOptions<SpectreLoggerOptions> optionsProvider)
        {
            _options = optionsProvider.Value;
        }

        /// <inheritdoc />
        public LogLevel MinimumLevel => _options.MinimumLevel;

        /// <inheritdoc />
        public FormattingProfile GetFormattingProfile(LogLevel logLevel)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public ITemplateRenderer[] GetRenderingPipeline(LogLevel logLevel)
        {
            throw new NotImplementedException();
        }
    }
}