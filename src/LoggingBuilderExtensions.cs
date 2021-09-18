using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Internal;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Rendering;
using Vertical.SpectreLogger.Scopes;
using Vertical.SpectreLogger.Templates;

namespace Vertical.SpectreLogger
{
    public static class LoggingBuilderExtensions
    {
        /// <summary>
        /// Adds the spectre console logging provider.
        /// </summary>
        /// <param name="builder">Logging builder instance</param>
        /// <param name="configureBuilder">A delegate that receives an options that can control
        /// how the logger renders events.</param>
        /// <returns><paramref name="builder"/></returns>
        public static ILoggingBuilder AddSpectreConsole(
            this ILoggingBuilder builder,
            Action<SpectreLoggingBuilder>? configureBuilder = null)
        {
            var services = builder.Services;
            var optionsBuilder = new SpectreLoggingBuilder(services);
            
            services.AddTransient<ITemplateRendererBuilder, TemplateRendererBuilder>();
            services.AddSingleton(AnsiConsole.Console);
            services.AddSingleton<ScopeManager>();
            services.AddSingleton<IRendererPipeline, RendererPipeline>();
            services.AddSingleton<ILoggerProvider, SpectreLoggerProvider>();

            optionsBuilder
                .AddTemplateRenderers()
                .WriteInForeground()
                .UseConsole(AnsiConsole.Console)
                .SetMinimumLevel(LogLevel.Information);

            configureBuilder?.Invoke(optionsBuilder);

            return builder;
        }
    }
}