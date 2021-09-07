using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Internal;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Templates;

namespace Vertical.SpectreLogger
{
    public static class LoggingBuilderExtensions
    {
        public static ILoggingBuilder AddSpectreConsole(
            this ILoggingBuilder builder,
            Action<SpectreLoggerBuilder>? configureBuilder = null)
        {
            var services = builder.Services;
            var optionsBuilder = new SpectreLoggerBuilder(services);
            
            services.AddTransient<ITemplateRendererBuilder, TemplateRendererBuilder>();
            services.AddSingleton(AnsiConsole.Console);
            services.AddSingleton<ScopeManager>();
            services.AddSingleton<IRendererPipeline, RendererPipeline>();
            services.AddSingleton<ILoggerProvider, SpectreLoggerProvider>();

            optionsBuilder.AddTemplateRenderers(typeof(LoggingBuilderExtensions).Assembly);
            
            configureBuilder?.Invoke(optionsBuilder);

            return builder;
        }
    }
}