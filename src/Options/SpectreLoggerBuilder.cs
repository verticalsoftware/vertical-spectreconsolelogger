using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Internal;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.Reflection;

namespace Vertical.SpectreLogger.Options
{
    /// <summary>
    /// Object used to configure the logger.
    /// </summary>
    public partial class SpectreLoggerBuilder
    {
        internal SpectreLoggerBuilder(IServiceCollection services)
        {
            Services = services;
            ConfigureDefaults();
        }

        /// <summary>
        /// Gets the application services collection.
        /// </summary>
        public IServiceCollection Services { get; }

        /// <summary>
        /// Sets the minimum log level.
        /// </summary>
        /// <param name="logLevel">Log level.</param>
        /// <returns>A reference to this instance.</returns>
        public SpectreLoggerBuilder SetMinimumLevel(LogLevel logLevel)
        {
            Services.Configure<SpectreLoggerOptions>(opt => opt.MinimumLogLevel = logLevel);
            return this;
        }

        /// <summary>
        /// Sets an object that can filter log events from the rendering pipeline.
        /// </summary>
        /// <param name="eventFilter"></param>
        /// <returns></returns>
        public SpectreLoggerBuilder SetLogEventFilter(ILogEventFilter eventFilter)
        {
            Services.Configure<SpectreLoggerOptions>(opt => opt.LogEventFilter = eventFilter);
            return this;
        }

        /// <summary>
        /// Writes event data to the console on a background thread.
        /// </summary>
        /// <returns>A reference to this instance.</returns>
        public SpectreLoggerBuilder WriteInBackground()
        {
            Services.Replace(ServiceDescriptor.Singleton<IConsoleWriter, BackgroundConsoleWriter>());
            return this;
        }

        /// <summary>
        /// Writes event data to the console on the calling thread.
        /// </summary>
        /// <returns>A reference to this instance.</returns>
        public SpectreLoggerBuilder WriteInForeground()
        {
            Services.Replace(ServiceDescriptor.Singleton<IConsoleWriter, ForegroundConsoleWriter>());
            return this;
        }

        /// <summary>
        /// Sets the provided instance as the final output device.
        /// </summary>
        /// <param name="console">The console to output log events to.</param>
        /// <returns>A reference to this instance.</returns>
        public SpectreLoggerBuilder UseConsole(IAnsiConsole console)
        {
            Services.AddSingleton(console);
            return this;
        }

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
        /// <returns>A reference to this instance</returns>
        public SpectreLoggerBuilder ConfigureProfile(LogLevel logLevel, Action<LogLevelProfile> configureProfile)
        {
            Services.Configure<SpectreLoggerOptions>(options => configureProfile(options.LogLevelProfiles[logLevel]));
            return this;
        }

        /// <summary>
        /// Adds a template renderer.
        /// </summary>
        /// <param name="rendererType">Type that implements <see cref="ITemplateRenderer"/>.</param>
        /// <returns>A reference to this instance</returns>
        public SpectreLoggerBuilder AddTemplateRenderer(Type rendererType)
        {
            Services.AddSingleton(new TemplateDescriptor(rendererType));
            return this;
        }

        /// <summary>
        /// Adds a template renderer.
        /// </summary>
        /// <typeparam name="T">Type that implements <see cref="ITemplateRenderer"/></typeparam>
        /// <returns>A reference to this instance</returns>
        public SpectreLoggerBuilder AddTemplateRenderer<T>() where T : ITemplateRenderer => AddTemplateRenderer(typeof(T));

        /// <summary>
        /// Adds all public template renderers found in an assembly.
        /// </summary>
        /// <param name="assembly">The assembly to scan. If not provided, the calling assembly is used.</param>
        /// <returns>A reference to this instance</returns>
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
    }
}