using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Formatting;
using Vertical.SpectreLogger.Internal;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.Reflection;
using Vertical.SpectreLogger.Templates;

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

            services.AddTransient<ITemplateRendererBuilder, TemplateRendererBuilder>();
            services.AddSingleton(AnsiConsole.Console);
            services.AddSingleton<ScopeManager>();
            services.AddSingleton<IRendererPipeline, RendererPipeline>();
            services.AddSingleton<ILoggerProvider, SpectreLoggerProvider>();

            AddTemplateRenderers(typeof(SpectreLoggerBuilder).Assembly);

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

        /// <summary>
        /// Adds a formatter for the given types to all log level profiles.
        /// </summary>
        /// <param name="types">The types to apply the formatter to</param>
        /// <param name="customFormatter">Custom formatter</param>
        /// <returns>A reference to this instance</returns>
        public SpectreLoggerBuilder AddTypeFormatter(
            IEnumerable<Type> types,
            ICustomFormatter customFormatter)
        {
            return ConfigureProfiles(profile =>
            {
                foreach (var type in types)
                {
                    profile.TypeFormatters[type] = customFormatter;
                }
            });
        }

        /// <summary>
        /// Adds a formatter for the given types to a single log level profile.
        /// </summary>
        /// <param name="logLevel">Log level</param>
        /// <param name="types">The types to apply the formatter to</param>
        /// <param name="customFormatter">Custom formatter</param>
        /// <returns>A reference to this instance</returns>
        public SpectreLoggerBuilder AddTypeFormatter(LogLevel logLevel,
            IEnumerable<Type> types,
            ICustomFormatter customFormatter)
        {
            return ConfigureProfile(logLevel, profile =>
            {
                foreach (var type in types)
                {
                    profile.TypeFormatters[type] = customFormatter;
                }
            });
        }

        /// <summary>
        /// Adds a formatter for the given type to a single log level profile.
        /// </summary>
        /// <param name="logLevel">Log level</param>
        /// <param name="customFormatter">Custom formatter</param>
        /// <typeparam name="T">Type to format</typeparam>
        /// <returns>A reference to this instance</returns>
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
        /// <returns>A reference to this instance</returns>
        /// <exception cref="ArgumentNullException"><paramref name="customFormatter"/> is null.</exception>
        public SpectreLoggerBuilder AddTypeFormatter<T>(ICustomFormatter customFormatter)
        {
            return ConfigureProfiles(profile => profile.TypeFormatters[typeof(T)] = customFormatter
                ?? throw new ArgumentNullException(nameof(customFormatter)));
        }

        /// <summary>
        /// Adds a formatting delegate for the given type to all log level profiles.
        /// </summary>
        /// <param name="formatter">A delegate that performs formatting.</param>
        /// <typeparam name="T">Value type</typeparam>
        /// <returns>A reference to this instance</returns>
        public SpectreLoggerBuilder AddTypeFormatter<T>(ValueFormatter formatter) => AddTypeFormatter<T>(
            new DelegateBackingFormatter(formatter));

        /// <summary>
        /// Adds a formatting delegate for the given type to all log level profiles.
        /// </summary>
        /// <param name="logLevel">The log level to apply the formatter to.</param>
        /// <param name="formatter">A delegate that performs formatting.</param>
        /// <typeparam name="T">Value type</typeparam>
        /// <returns>A reference to this instance</returns>
        public SpectreLoggerBuilder AddTypeFormatter<T>(LogLevel logLevel, ValueFormatter formatter) =>
            AddTypeFormatter<T>(logLevel, new DelegateBackingFormatter(formatter));

        /// <summary>
        /// Adds a formatting delegate for the given types for the specified log level.
        /// </summary>
        /// <param name="logLevel">The log level to apply the formatter to.</param>
        /// <param name="types">The types to apply the formatting to.</param>
        /// <param name="formatter">A delegate that performs formatting.</param>
        /// <returns>A reference to this instance.</returns>
        public SpectreLoggerBuilder AddTypeFormatter(
            LogLevel logLevel,
            IEnumerable<Type> types,
            ValueFormatter formatter)
        {
            return AddTypeFormatter(logLevel, types, new DelegateBackingFormatter(formatter));
        }

        /// <summary>
        /// Adds a formatting delegate for the given types for all log levels.
        /// </summary>
        /// <param name="types">The types to apply the formatting to.</param>
        /// <param name="formatter">A delegate that performs formatting.</param>
        /// <returns>A reference to this instance.</returns>
        public SpectreLoggerBuilder AddTypeFormatter(
            IEnumerable<Type> types,
            ValueFormatter formatter)
        {
            return AddTypeFormatter(types, new DelegateBackingFormatter(formatter));
        }
        
        /// <summary>
        /// Registers markup that is written just before a specific value is rendered for the given log level.
        /// The markup closing tag is written automatically.
        /// </summary>
        /// <param name="logLevel">Log level to apply the value style for</param>
        /// <param name="value">Value to style</param>
        /// <param name="style">Markup that is written before this value is rendered</param>
        /// <typeparam name="TValue">Value type</typeparam>
        /// <returns>A reference to this instance</returns>
        public SpectreLoggerBuilder AddValueStyle<TValue>(LogLevel logLevel, TValue value, string style)
            where TValue : notnull
        {
            return ConfigureProfile(logLevel, profile => profile.ValueStyles[value] = style);
        }

        /// <summary>
        /// Registers markup that is written just before a specific value is rendered for all log levels.
        /// The markup closing tag is written automatically.
        /// </summary>
        /// <param name="value">Value to style</param>
        /// <param name="style">Markup that is written before this value is rendered</param>
        /// <typeparam name="TValue">Value type</typeparam>
        /// <returns>A reference to this instance</returns>
        public SpectreLoggerBuilder AddValueStyle<TValue>(TValue value, string style) where TValue : notnull
        {
            return ConfigureProfiles(profile => profile.ValueStyles[value] = style);
        }

        /// <summary>
        /// Registers markup that is written just before a value of a specific type is rendered for the given
        /// log level.
        /// </summary>
        /// <param name="logLevel">Log level to apply the type style for</param>
        /// <param name="type">The type to associate with the style</param>
        /// <param name="style">Markup that is written before the value is rendered</param>
        /// <returns>A reference to this instance</returns>
        public SpectreLoggerBuilder AddTypeStyle(LogLevel logLevel, Type type, string style)
        {
            return ConfigureProfile(logLevel, profile => profile.TypeStyles[type] = style);
        }

        /// <summary>
        /// Registers markup that is written just before a value of a specific type is rendered for all
        /// log levels.
        /// </summary>
        /// <param name="type">The type to associate with the style</param>
        /// <param name="style">Markup that is written before the value is rendered</param>
        /// <returns>A reference to this instance</returns>
        public SpectreLoggerBuilder AddTypeStyle(Type type, string style)
        {
            return ConfigureProfiles(profile => profile.TypeStyles[type] = style);
        }
        
        /// <summary>
        /// Registers markup that is written just before a value of a specific type is rendered for the given
        /// log level.
        /// </summary>
        /// <param name="logLevel">Log level to apply the type style for</param>
        /// <param name="types">The types to associate with the style</param>
        /// <param name="style">Markup that is written before the value is rendered</param>
        /// <returns>A reference to this instance</returns>
        public SpectreLoggerBuilder AddTypeStyle(LogLevel logLevel, IEnumerable<Type> types, string style)
        {
            foreach (var type in types)
            {
                AddTypeStyle(logLevel, type, style);
            }

            return this;
        }
        
        /// <summary>
        /// Registers markup that is written just before a value of a specific type is rendered all log levels.
        /// </summary>
        /// <param name="types">The types to associate with the style</param>
        /// <param name="style">Markup that is written before the value is rendered</param>
        /// <returns>A reference to this instance</returns>
        public SpectreLoggerBuilder AddTypeStyle(IEnumerable<Type> types, string style)
        {
            foreach (var type in types)
            {
                AddTypeStyle(type, style);
            }

            return this;
        }

        /// <summary>
        /// Registers markup that is written just before a value of a specific type is rendered for the given
        /// log level.
        /// </summary>
        /// <param name="logLevel">Log level to apply the type style for</param>
        /// <param name="style">Markup that is written before the value is rendered</param>
        /// <typeparam name="T">The type</typeparam>
        /// <returns>A reference to this instance</returns>
        public SpectreLoggerBuilder AddTypeStyle<T>(LogLevel logLevel, string style)
        {
            return AddTypeStyle(logLevel, typeof(T), style);
        }
        
        /// <summary>
        /// Registers markup that is written just before a value of a specific type is rendered for all
        /// log levels.
        /// </summary>
        /// <param name="style">Markup that is written before the value is rendered</param>
        /// <typeparam name="T">The type</typeparam>
        /// <returns>A reference to this instance</returns>
        public SpectreLoggerBuilder AddTypeStyle<T>(string style)
        {
            return AddTypeStyle(typeof(T), style);
        }

        /// <summary>
        /// Configures the options of a renderer.
        /// </summary>
        /// <param name="logLevel">Log level</param>
        /// <param name="configure">Delegate that configures the provided options object</param>
        /// <typeparam name="TOptions">Options type</typeparam>
        /// <returns>A reference to this instance</returns>
        public SpectreLoggerBuilder ConfigureRenderer<TOptions>(LogLevel logLevel, Action<TOptions> configure) 
            where TOptions : IRendererOptions, new()
        {
            return ConfigureProfile(logLevel, profile => profile.RendererOptions.Configure(configure));
        }
    }
}