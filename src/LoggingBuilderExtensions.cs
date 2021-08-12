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