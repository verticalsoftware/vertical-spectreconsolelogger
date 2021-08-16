using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Spectre.Console;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Infrastructure;
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
            Action<SpectreLoggerOptions>? configureOptions = null)
        {
            var services = builder.Services;
            
            services.Configure<SpectreLoggerOptions>(options =>
            {
                options.AddRenderers(Assembly.GetExecutingAssembly());
                configureOptions?.Invoke(options);
            });

            services.AddSingleton(AnsiConsole.Console);
            services.AddSingleton<IWriteBufferFactory>(sp => new PooledWriteBufferFactory(sp.GetRequiredService<IAnsiConsole>()));
            services.AddSingleton(AnsiConsole.Console);
            services.AddSingleton<ITemplateRendererFactory, TemplateRendererFactory>();
            services.AddSingleton<ILoggerProvider, SpectreLoggerProvider>();
            services.AddSingleton<IScopeManager, ScopeManager>();
            services.AddSingleton<ILogLevelController>(sp => sp.GetRequiredService<IOptions<SpectreLoggerOptions>>().Value.LogLevelController
                                                             ?? new DefaultLogLevelController(LogLevel.Trace));

            return builder;
        }
    }
}