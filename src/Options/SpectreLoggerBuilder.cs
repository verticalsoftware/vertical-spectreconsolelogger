using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Internal;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.Reflection;
using Vertical.SpectreLogger.Templates;

namespace Vertical.SpectreLogger.Options
{
    /// <summary>
    /// Object used to configure the logger.
    /// </summary>
    public class SpectreLoggerBuilder
    {
        internal SpectreLoggerBuilder(IServiceCollection services)
        {
            Services = services;

            services.AddTransient<ITemplateRendererBuilder, TemplateRendererBuilder>();
            services.AddSingleton(AnsiConsole.Console);
            services.AddSingleton<IAnsiConsoleWriter, BackgroundAnsiConsoleWriter>();
            services.AddSingleton<ScopeManager>();
            services.AddSingleton<IRendererPipeline, RendererPipeline>();
            services.AddSingleton<ILoggerProvider, SpectreLoggerProvider>();

            AddTemplateRenderers(typeof(SpectreLoggerBuilder).Assembly);
        }

        /// <summary>
        /// Gets the application services collection.
        /// </summary>
        public IServiceCollection Services { get; }

        /// <summary>
        /// Configures settings for all log profiles.
        /// </summary>
        /// <param name="configureProfile">Delegate that performs the configuration.</param>
        /// <returns><see cref="SpectreLoggerOptions"/></returns>
        /// <remarks>
        /// This method calls the configuration delegate for each log level profile.
        /// </remarks>
        public SpectreLoggerBuilder ConfigureProfiles(Action<LogLevelProfile> configureProfile)
        {
            Services.Configure<SpectreLoggerOptions>(options =>
            {
                foreach (var profile in options.LogLevelProfiles.Values)
                {
                    configureProfile(profile);
                }
            });
            return this;
        }

        /// <summary>
        /// Configures a specific log level profile.
        /// </summary>
        /// <param name="logLevel">Log level.</param>
        /// <param name="configureProfile">Delegate that performs the configuration.</param>
        /// <returns><see cref="SpectreLoggerBuilder"/></returns>
        public SpectreLoggerBuilder ConfigureProfile(LogLevel logLevel, Action<LogLevelProfile> configureProfile)
        {
            Services.Configure<SpectreLoggerOptions>(options => configureProfile(options.LogLevelProfiles[logLevel]));
            return this;
        }

        /// <summary>
        /// Adds a template renderer.
        /// </summary>
        /// <param name="rendererType">Type that implements <see cref="ITemplateRenderer"/>.</param>
        /// <returns><see cref="SpectreLoggerBuilder"/></returns>
        public SpectreLoggerBuilder AddTemplateRenderer(Type rendererType)
        {
            Services.AddSingleton(new RendererDescriptor(rendererType));
            return this;
        }

        /// <summary>
        /// Adds a template renderer.
        /// </summary>
        /// <typeparam name="T">Type that implements <see cref="ITemplateRenderer"/></typeparam>
        /// <returns><see cref="SpectreLoggerBuilder"/></returns>
        public SpectreLoggerBuilder AddTemplateRenderer<T>() where T : ITemplateRenderer => AddTemplateRenderer(typeof(T));

        /// <summary>
        /// Adds all public template renderers found in an assembly.
        /// </summary>
        /// <param name="assembly">The assembly to scan. If not provided, the calling assembly is used.</param>
        /// <returns><see cref="SpectreLoggerBuilder"/></returns>
        public SpectreLoggerBuilder AddTemplateRenderers(Assembly? assembly)
        {
            assembly ??= Assembly.GetCallingAssembly();

            foreach (var type in assembly.GetTypes())
            {
                if (TypeActivator.CanCreateInstanceOfType<ITemplateRenderer>(type, out _))
                {
                    AddTemplateRenderer(type);
                }
            }

            return this;
        }

        /// <summary>
        /// Adds a formatter for the given type to a single log level profile.
        /// </summary>
        /// <param name="logLevel">Log level</param>
        /// <param name="customFormatter">Custom formatter</param>
        /// <typeparam name="T">Type to format</typeparam>
        /// <returns><see cref="SpectreLoggerBuilder"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="customFormatter"/> is null.</exception>
        public SpectreLoggerBuilder AddTypeFormatter<T>(LogLevel logLevel, ICustomFormatter customFormatter)
        {
            return ConfigureProfile(logLevel, profile => profile.TypeFormatters[typeof(T)] = 
                customFormatter ?? throw new ArgumentNullException(nameof(customFormatter)));
        }

        /// <summary>
        /// Adds a formatter for the given type to all log level profiles.
        /// </summary>
        /// <param name="customFormatter">Custom formatter</param>
        /// <typeparam name="T">Type to format</typeparam>
        /// <returns><see cref="SpectreLoggerBuilder"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="customFormatter"/> is null.</exception>
        public SpectreLoggerBuilder AddTypeFormatter<T>(ICustomFormatter customFormatter)
        {
            return ConfigureProfiles(profile => profile.TypeFormatters[typeof(T)] = customFormatter
                ?? throw new ArgumentNullException(nameof(customFormatter)));
        }
    }
}