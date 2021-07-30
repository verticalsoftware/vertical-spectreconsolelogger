using System;
using System.Linq;
using System.Reflection;
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
            services.AddSingleton<ITemplateRendererBuilder, TemplateRendererBuilder>();
            services.AddSingleton<IWriteBufferFactory, DefaultWriteBufferFactory>();
            services.AddSingleton(AnsiConsole.Console);

            loggingBuilder.AddSpectreConsoleRenderersFromAssembly(typeof(LoggingBuilderExtensions).Assembly);

            return loggingBuilder;
        }

        /// <summary>
        /// Adds all implemented template renderers from the given assembly to
        /// the logging configuration.
        /// </summary>
        /// <param name="loggingBuilder">Logging builder.</param>
        /// <param name="assembly">Assembly</param>
        /// <returns><see cref="ILoggingBuilder"/></returns>
        public static ILoggingBuilder AddSpectreConsoleRenderersFromAssembly(this ILoggingBuilder loggingBuilder,
            Assembly assembly)
        {
            var rendererTypes = assembly
                .GetTypes()
                .Where(t => t.IsPublic && t.IsClass && typeof(ITemplateRenderer).IsAssignableFrom(t));

            foreach (var type in rendererTypes)
            {
                loggingBuilder.AddSpectreConsoleRenderer(type);
            }

            return loggingBuilder;
        }

        /// <summary>
        /// Adds a custom template renderer.
        /// </summary>
        /// <param name="loggingBuilder">Logging builder</param>
        /// <typeparam name="T">The type of template renderer to register.</typeparam>
        /// <returns><see cref="ILoggingBuilder"/></returns>
        public static ILoggingBuilder AddSpectreConsoleRenderer<T>(this ILoggingBuilder loggingBuilder)
            where T : class, ITemplateRenderer
        {
            return loggingBuilder.AddSpectreConsoleRenderer(typeof(T));
        }

        private static ILoggingBuilder AddSpectreConsoleRenderer(this ILoggingBuilder loggingBuilder,
            Type rendererType)
        {
            loggingBuilder.Services.AddSingleton(new TemplateDescriptor(rendererType));
            return loggingBuilder;
        }
    }
}