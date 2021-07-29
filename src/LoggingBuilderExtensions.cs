using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Vertical.SpectreLogger.Memory;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.Rendering;

namespace Vertical.SpectreLogger
{
    public static class LoggingBuilderExtensions
    {
        /// <summary>
        /// Adds the <see cref="SpectreLoggerProvider"/> to the logging configuration.
        /// </summary>
        /// <param name="loggingBuilder">Logging builder</param>
        /// <param name="configureOptions">Action used to configure the logger options.</param>
        /// <returns><see cref="ILoggingBuilder"/></returns>
        public static ILoggingBuilder AddSpectreConsole(this ILoggingBuilder loggingBuilder,
            Action<SpectreLoggerOptions> configureOptions)
        {
            var services = loggingBuilder.Services;
            
            services.Configure(configureOptions);
            services.AddSingleton<ILoggerProvider, SpectreLoggerProvider>();
            services.AddSingleton<IStringBuilderPool>(new StringBuilderPool(5));
            services.AddSingleton<ITemplateRendererProvider, TemplateRendererProvider>();
            services.AddSingleton<IWriteBufferFactory, DefaultWriteBufferFactory>();
            services.AddSingleton(AnsiConsole.Console);

            var formatterTypes = typeof(SpectreLogger)
                .Assembly
                .GetTypes()
                .Where(type => typeof(ITemplateRenderer).IsAssignableFrom(type))
                .Where(type => !type.IsAbstract && type.IsClass && type.IsPublic);

            foreach (var formatterType in formatterTypes)
            {
                services.AddSingleton(typeof(ITemplateRenderer), formatterType);
            }

            return loggingBuilder;
        }

        /// <summary>
        /// Adds a custom template renderer.
        /// </summary>
        /// <param name="loggingBuilder">Logging builder</param>
        /// <typeparam name="T">The type of template renderer to register.</typeparam>
        /// <returns><see cref="ILoggingBuilder"/></returns>
        public static ILoggingBuilder AddSpectreConsoleFormatter<T>(this ILoggingBuilder loggingBuilder)
            where T : class, ITemplateRenderer
        {
            loggingBuilder.Services.AddSingleton<ITemplateRenderer, T>();
            return loggingBuilder;
        }

        /// <summary>
        /// Adds a custom template renderer instance.
        /// </summary>
        /// <param name="loggingBuilder">The logging builder instance.</param>
        /// <param name="renderer">The formatter instance.</param>
        /// <returns><see cref="ILoggingBuilder"/></returns>
        public static ILoggingBuilder AddSpectreConsoleFormatter(this ILoggingBuilder loggingBuilder, 
            ITemplateRenderer renderer)
        {
            loggingBuilder.Services.AddSingleton<ITemplateRenderer>(renderer);
            return loggingBuilder;
        }
    }
}