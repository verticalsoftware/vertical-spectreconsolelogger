using System;
using Microsoft.Extensions.Logging;
using Vertical.SpectreLogger.Options;

namespace Vertical.SpectreLogger
{
    public static class LoggingBuilderExtensions
    {
        public static ILoggingBuilder AddSpectreConsole(
            this ILoggingBuilder builder,
            Action<SpectreLoggerBuilder>? configureLogger = null)
        {
            var loggerBuilder = new SpectreLoggerBuilder(builder.Services);
            
            configureLogger?.Invoke(loggerBuilder);

            return builder;
        }
    }
}