using System;
using System.Collections.Generic;
using Vertical.SpectreLogger.Internal;
using Vertical.SpectreLogger.MatchableTypes;

namespace Vertical.SpectreLogger.Options
{
    public static class FormattingProfileExtensions
    {
        /// <summary>
        /// Adds markup that styles the rendering of specific object types.
        /// </summary>
        /// <param name="formattingProfile">Formatting profile</param>
        /// <typeparam name="T">Type to markup</typeparam>
        /// <param name="markup">Markup</param>
        /// <returns><see cref="FormattingProfile"/></returns>
        public static FormattingProfile AddTypeStyle<T>(this FormattingProfile formattingProfile,
            string markup) => formattingProfile.AddTypeStyle(typeof(T), markup);
        
        /// <summary>
        /// Adds markup that styles the rendering of specific object types.
        /// </summary>
        /// <param name="formattingProfile">Formatting profile</param>
        /// <param name="type">Type to markup</param>
        /// <param name="markup">Markup</param>
        /// <returns><see cref="FormattingProfile"/></returns>
        public static FormattingProfile AddTypeStyle(this FormattingProfile formattingProfile, 
            Type type, 
            string markup)
        {
            formattingProfile.TypeStyles[type] = markup;
            return formattingProfile;
        }

        /// <summary>
        /// Adds markup that styles the rendering of specific object types.
        /// </summary>
        /// <param name="formattingProfile">Formatting profile</param>
        /// <param name="types">Type to markup</param>
        /// <param name="markup">Markup</param>
        /// <returns><see cref="FormattingProfile"/></returns>
        public static FormattingProfile AddTypeStyle(this FormattingProfile formattingProfile,
            IEnumerable<Type> types,
            string markup)
        {
            foreach (var type in types)
            {
                formattingProfile.AddTypeStyle(type, markup);
            }

            return formattingProfile;
        }

        /// <summary>
        /// Clears all type style formatting.
        /// </summary>
        /// <param name="formattingProfile">Formatting profile</param>
        /// <returns><see cref="FormattingProfile"/></returns>
        public static FormattingProfile ClearTypeStyles(this FormattingProfile formattingProfile)
        {
            formattingProfile.TypeStyles.Clear();
            return formattingProfile;
        }

        /// <summary>
        /// Adds a function that formats the output of a specific value type.
        /// </summary>
        /// <param name="formattingProfile">Formatting profile.</param>
        /// <param name="type">The value type to format.</param>
        /// <param name="formatter">A function that returns a string representation of the given value.</param>
        /// <returns><see cref="FormattingProfile"/></returns>
        public static FormattingProfile AddTypeFormatter(this FormattingProfile formattingProfile,
            Type type,
            Func<object?, string?> formatter)
        {
            formattingProfile.TypeFormatters[type] = formatter;
            return formattingProfile;
        }

        /// <summary>
        /// Adds a function that formats the output of specific value types.
        /// </summary>
        /// <param name="formattingProfile">Formatting profile.</param>
        /// <param name="types">The value type to format.</param>
        /// <param name="formatter">A function that returns a string representation of the given value.</param>
        /// <returns><see cref="FormattingProfile"/></returns>
        public static FormattingProfile AddValueFormatters(this FormattingProfile formattingProfile,
            IEnumerable<Type> types,
            Func<object?, string?> formatter)
        {
            foreach (var type in types)
            {
                formattingProfile.AddTypeFormatter(type, formatter);
            }

            return formattingProfile;
        }

        /// <summary>
        /// Adds a function that formats the output of a specific value type.
        /// </summary>
        /// <param name="formattingProfile">Formatting profile.</param>
        /// <typeparam name="T">The value type to format.</typeparam>
        /// <param name="formatter">A function that returns a string representation of the given value.</param>
        /// <returns><see cref="FormattingProfile"/></returns>
        public static FormattingProfile AddTypeFormatter<T>(this FormattingProfile formattingProfile,
            Func<object?, string?> formatter) => formattingProfile.AddTypeFormatter(typeof(T), formatter);

        /// <summary>
        /// Clears all type value formatting functions.
        /// </summary>
        /// <param name="formattingProfile">Formatting profile</param>
        /// <returns><see cref="FormattingProfile"/></returns>
        public static FormattingProfile ClearValueFormatters(this FormattingProfile formattingProfile)
        {
            formattingProfile.TypeFormatters.Clear();
            return formattingProfile;
        }

        /// <summary>
        /// Renders log level name using the value obtained by calling the ToString() method on the
        /// log level value.
        /// </summary>
        /// <param name="formattingProfile">Formatting profile</param>
        /// <returns><see cref="FormattingProfile"/></returns>
        public static FormattingProfile RenderVerboseLogLevelNames(this FormattingProfile formattingProfile)
        {
            formattingProfile.LogLevelDisplay = formattingProfile.LogLevel.ToString();
            return formattingProfile;
        }
        
        /// <summary>
        /// Configures options for a specific rendering options type.
        /// </summary>
        /// <param name="formattingProfile">Formatting profile.</param>
        /// <param name="configure">Action that configures the given options object.</param>
        /// <typeparam name="TOptions">Options type.</typeparam>
        /// <returns><see cref="FormattingProfile"/></returns>
        public static FormattingProfile ConfigureRendererOptions<TOptions>(this FormattingProfile formattingProfile,
            Action<TOptions> configure) where TOptions : class, new()
        {
            if (!formattingProfile.RendererOptions.TryGetValue(typeof(TOptions), out var optionsObj))
            {
                optionsObj = new TOptions();
                formattingProfile.RendererOptions.Add(typeof(TOptions), optionsObj);
            }

            configure((TOptions) optionsObj);

            return formattingProfile;
        }

        /// <summary>
        /// Gets the current rendering options of the specified type.
        /// </summary>
        /// <param name="formattingProfile">Formatting profile.</param>
        /// <typeparam name="TOptions">Options type</typeparam>
        /// <returns>The options instance or null if never configured.</returns>
        public static TOptions? GetRenderingOptions<TOptions>(this FormattingProfile formattingProfile)
            where TOptions : class
        {
            return formattingProfile.RendererOptions.GetValueOrDefault(typeof(TOptions)) as TOptions;
        }
    }
}