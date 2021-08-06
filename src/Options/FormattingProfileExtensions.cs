using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Vertical.SpectreLogger.Internal;
using Vertical.SpectreLogger.PseudoTypes;

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
        /// Adds a style for a specific value.
        /// </summary>
        /// <param name="formattingProfile">Formatting profile</param>
        /// <param name="value">The value the style is applied to</param>
        /// <param name="markup">The markup to apply before rendering the value</param>
        /// <typeparam name="T">The value type</typeparam>
        /// <returns><see cref="FormattingProfile"/></returns>
        public static FormattingProfile AddValueStyle<T>(this FormattingProfile formattingProfile,
            T value,
            string markup) where T : notnull
        {
            formattingProfile.ValueStyles[(typeof(T), (object)value)] = markup;
            return formattingProfile;
        }

        /// <summary>
        /// Adds a style for specific values.
        /// </summary>
        /// <param name="formattingProfile">Formatting profile</param>
        /// <param name="values">The value the style is applied to</param>
        /// <param name="markup">The markup to apply before rendering any of the provided values</param>
        /// <returns><see cref="FormattingProfile"/></returns>
        /// <exception cref="ArgumentException"><paramref name="values"/> contains a null value.</exception>
        public static FormattingProfile AddValueStyles(this FormattingProfile formattingProfile,
            IEnumerable<object> values,
            string markup)
        {
            var valueStyles = formattingProfile.ValueStyles;
            
            foreach (var obj in values)
            {
                var type = obj?.GetType() ?? throw new ArgumentException("Null values are not allowed.", nameof(values));
                valueStyles[(type, obj)] = markup;
            }

            return formattingProfile;
        }
        
        /// <summary>
        /// Clears all value styles.
        /// </summary>
        /// <param name="formattingProfile">Formatting profile</param>
        /// <returns><see cref="FormattingProfile"/></returns>
        public static FormattingProfile ClearValueStyles(this FormattingProfile formattingProfile)
        {
            formattingProfile.ValueStyles.Clear();
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
        public static FormattingProfile AddTypeFormatters(this FormattingProfile formattingProfile,
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
            Func<T?, string?> formatter) => formattingProfile.AddTypeFormatter(typeof(T), obj => formatter((T?)obj));
        

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
        /// Configures options for a specific rendering options type.
        /// </summary>
        /// <param name="formattingProfile">Formatting profile.</param>
        /// <param name="configure">Action that configures the given options object.</param>
        /// <typeparam name="TOptions">Options type.</typeparam>
        /// <returns><see cref="FormattingProfile"/></returns>
        public static FormattingProfile ConfigureRenderer<TOptions>(this FormattingProfile formattingProfile,
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