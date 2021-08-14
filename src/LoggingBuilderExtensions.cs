using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Output;

namespace Vertical.SpectreLogger
{
    public static class LoggingBuilderExtensions
    {
        /// <summary>
        /// Adds the provider that enables logging to Spectre console.
        /// </summary>
        /// <param name="builder">Logging builder option</param>
        /// <param name="configureOptions">A delegate that performs the configuration. If
        /// not given, all default options are used.</param>
        /// <returns><see cref="ILoggingBuilder"/></returns>
        public static ILoggingBuilder AddSpectreConsole(this ILoggingBuilder builder,
            Action<SpectreConsoleLoggerOptions>? configureOptions = null)
        {
            var services = builder.Services;
            
            services.Configure<SpectreConsoleLoggerOptions>(options => configureOptions?.Invoke(options));

            services.AddSingleton(AnsiConsole.Console);
            services.AddSingleton<IWriteBufferProvider, PooledWriteBufferProvider>();
            
            return builder;
        }
    }
}