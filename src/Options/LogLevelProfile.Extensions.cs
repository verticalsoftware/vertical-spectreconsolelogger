using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Formatting;

namespace Vertical.SpectreLogger.Options
{
    public static class LogLevelProfileExtensions
    {
        /// <summary>
        /// Adds type formatters to the configuration that are decorated with
        /// <see cref="TypeFormatterAttribute"/>
        /// </summary>
        /// <param name="profile">Log level profile</param>
        /// <param name="assembly">The assembly to scan for formatters</param>
        /// <returns><see cref="LogLevelProfile"/></returns>
        /// <exception cref="InvalidOperationException">One of the discovered types could not be created.</exception>
        public static LogLevelProfile AddTypeFormatters(
            this LogLevelProfile profile,
            Assembly? assembly = null)
        {
            var formatterTypes = (assembly ?? Assembly.GetCallingAssembly())
                .ExportedTypes
                .Select(type => (type, attribute: type.GetCustomAttribute<TypeFormatterAttribute>()))
                .Where(item => item.attribute != null);

            foreach (var item in formatterTypes)
            {
                try
                {
                    profile.AddTypeFormatter(item.type, (ICustomFormatter) Activator.CreateInstance(item.type));
                }
                catch (Exception exception)
                {
                    throw new InvalidOperationException(
                        $"Could not create an instance of formatter type {item.type}",
                        exception);
                }
            }

            return profile;
        }
        
        /// <summary>
        /// Associates an <see cref="ICustomFormatter"/> instance with the given type.
        /// </summary>
        /// <param name="profile">Log level profile</param>
        /// <param name="type">The type to associate the formatter to.</param>
        /// <param name="formatter">The custom formatter instance.</param>
        /// <returns><see cref="LogLevelProfile"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="formatter"/> is null.</exception>
        public static LogLevelProfile AddTypeFormatter(
            this LogLevelProfile profile,
            Type type, 
            ICustomFormatter formatter)
        {
            profile.TypeFormatters[type ?? throw new ArgumentNullException(nameof(type))] =
                formatter ?? throw new ArgumentNullException(nameof(formatter));
            
            return profile;
        }
        
        /// <summary>
        /// Associates an <see cref="ICustomFormatter"/> instance with the given type.
        /// </summary>
        /// <param name="profile">Log level profile</param>
        /// <param name="type">The type to associate the formatter to.</param>
        /// <param name="formatter">The custom formatter delegate.</param>
        /// <returns><see cref="LogLevelProfile"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="formatter"/> is null.</exception>
        public static LogLevelProfile AddTypeFormatter(
            this LogLevelProfile profile,
            Type type, 
            ValueFormatter formatter)
        {
            var delegateFormatter = new DelegateBackingFormatter(
                formatter ?? throw new ArgumentNullException(nameof(formatter)));
            
            profile.TypeFormatters[type ?? throw new ArgumentNullException(nameof(type))] =
                 delegateFormatter;
            
            return profile;
        }

        /// <summary>
        /// Associates an <see cref="ICustomFormatter"/> instance with the given types.
        /// </summary>
        /// <param name="profile">Log level profile</param>
        /// <param name="types">The types to associate the formatter to.</param>
        /// <param name="formatter">The custom formatter instance.</param>
        /// <returns><see cref="LogLevelProfile"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="types"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="formatter"/> is null.</exception>
        public static LogLevelProfile AddTypeFormatter(
            this LogLevelProfile profile,
            IEnumerable<Type> types,
            ICustomFormatter formatter)
        {
            foreach (var type in types ?? throw new ArgumentNullException(nameof(types)))
            {
                profile.AddTypeFormatter(type, formatter);
            }

            return profile;
        }
        
        /// <summary>
        /// Associates an <see cref="ICustomFormatter"/> instance with the given types.
        /// </summary>
        /// <param name="profile">Log level profile</param>
        /// <param name="types">The types to associate the formatter to.</param>
        /// <param name="formatter">The formatter delegate.</param>
        /// <returns><see cref="LogLevelProfile"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="formatter"/> is null.</exception>
        public static LogLevelProfile AddTypeFormatter(
            this LogLevelProfile profile,
            IEnumerable<Type> types,
            ValueFormatter formatter)
        {
            foreach (var type in types ?? throw new ArgumentNullException(nameof(types)))
            {
                profile.AddTypeFormatter(type, formatter);
            }

            return profile;
        }

        /// <summary>
        /// Associates an <see cref="ICustomFormatter"/> instance with the given type.
        /// </summary>
        /// <param name="profile">Log level profile</param>
        /// <typeparam name="T">The type to associate the formatter to.</typeparam>
        /// <param name="formatter">The custom formatter instance.</param>
        /// <returns><see cref="LogLevelProfile"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="formatter"/> is null.</exception>
        public static LogLevelProfile AddTypeFormatter<T>(
            this LogLevelProfile profile,
            ICustomFormatter formatter)
            where T : notnull
        {
            return profile.AddTypeFormatter(typeof(T), formatter);
        }
        
        /// <summary>
        /// Associates an <see cref="ICustomFormatter"/> instance with the given type.
        /// </summary>
        /// <param name="profile">Log level profile</param>
        /// <typeparam name="T">The type to associate the formatter to.</typeparam>
        /// <param name="formatter">The formatter delegate.</param>
        /// <returns><see cref="LogLevelProfile"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="formatter"/> is null.</exception>
        public static LogLevelProfile AddTypeFormatter<T>(
            this LogLevelProfile profile,
            ValueFormatter formatter)
            where T : notnull
        {
            return profile.AddTypeFormatter(typeof(T), formatter);
        }

        /// <summary>
        /// Adds markup that is used to style the rendering of a specific value.
        /// </summary>
        /// <param name="profile">Log level profile</param>
        /// <param name="value">The value to associate with the style.</param>
        /// <param name="markup">The markup to write prior to rendering the value.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="markup"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="markup"/> is whitespace.</exception>
        public static LogLevelProfile AddValueStyle(
            this LogLevelProfile profile,
            object value, 
            string markup)
        {
            if (string.IsNullOrWhiteSpace(markup))
            {
                throw new ArgumentException("Markup cannot be null/whitespace", nameof(markup));
            }
            
            profile.ValueStyles[value ?? throw new ArgumentNullException(nameof(value))] = markup;
            return profile;
        }

        /// <summary>
        /// Adds markup that is used to style the rendering of values of the given type.
        /// </summary>
        /// <param name="profile">Log level profile</param>
        /// <param name="type">The type to associate with the style.</param>
        /// <param name="markup">The markup to write prior to rendering the value.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="type"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="markup"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="markup"/> is whitespace.</exception>
        public static LogLevelProfile AddTypeStyle(
            this LogLevelProfile profile,
            Type type,
            string markup)
        {
            if (string.IsNullOrWhiteSpace(markup))
            {
                throw new ArgumentException("Markup cannot be null/whitespace", nameof(markup));
            }

            profile.TypeStyles[type ?? throw new ArgumentNullException(nameof(type))] = markup;
            return profile;
        }

        /// <summary>
        /// Adds markup that is used to style the rendering of values of the given types.
        /// </summary>
        /// <param name="profile">Log level profile</param>
        /// <param name="types">The types to associate with the style.</param>
        /// <param name="markup">The markup to write prior to rendering the value.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="types"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="markup"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="markup"/> is whitespace.</exception>
        public static LogLevelProfile AddTypeStyle(
            this LogLevelProfile profile,
            IEnumerable<Type> types,
            string markup)
        {
            foreach (var type in types ?? throw new ArgumentNullException(nameof(types)))
            {
                profile.AddTypeStyle(type, markup);
            }

            return profile;
        }


        /// <summary>
        /// Adds markup that is used to style the rendering of values of the given type.
        /// </summary>
        /// <param name="profile">Log level profile</param>
        /// <param name="markup">The markup to write prior to rendering the value.</param>
        /// <typeparam name="T">The type to associate with the style.</typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="markup"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="markup"/> is whitespace.</exception>        
        public static LogLevelProfile AddTypeStyle<T>(
            this LogLevelProfile profile,
            string markup)
            where T : notnull
        {
            return profile.AddTypeStyle(typeof(T), markup);
        }

        /// <summary>
        /// Registers a delegate that provides configuration for a specific renderer.
        /// </summary>
        /// <param name="profile">Log level profile</param>
        /// <param name="configureOptions">A delegate that performs configuration on the provided options object.</param>
        /// <typeparam name="TOptions">Options type.</typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="configureOptions"/> is null.</exception>
        public static LogLevelProfile ConfigureRenderer<TOptions>(
            this LogLevelProfile profile,
            Action<TOptions> configureOptions)
            where TOptions : new()
        {
            if (configureOptions == null)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }
            
            profile.RendererOptions.Configure(configureOptions);
            return profile;
        }
    }
}