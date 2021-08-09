using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Vertical.SpectreLogger.Internal;
using Vertical.SpectreLogger.PseudoTypes;
using Vertical.SpectreLogger.Rendering;

namespace Vertical.SpectreLogger.Options
{
    public static class FormattingProfileExtensions
    {
        /// <summary>
        /// Gets the display name to render for the log level name.
        /// </summary>
        /// <param name="formattingProfile">Formatting profile.</param>
        /// <param name="name">Name to display</param>
        /// <returns><see cref="FormattingProfile"/></returns>
        public static FormattingProfile SetLogLevelDisplayName(this FormattingProfile formattingProfile, string name)
        {
            return formattingProfile.ConfigureRenderer<LogLevelRenderer.Options>(opt => opt.Formatter = _ => name);
        }

        /// <summary>
        /// Sets the default type style.
        /// </summary>
        /// <param name="formattingProfile">Formatting profile.</param>
        /// <param name="markup">Markup style</param>
        /// <returns><see cref="FormattingProfile"/></returns>
        public static FormattingProfile SetDefaultTypeStyle(this FormattingProfile formattingProfile, 
            string markup)
        {
            return formattingProfile.ConfigureMultiTypeRenderers(options => options.DefaultTypeStyle = markup);
        } 
        
        /// <summary>
        /// Adds markup that styles the rendering of specific object types.
        /// </summary>
        /// <param name="formattingProfile">Formatting profile</param>
        /// <typeparam name="T">Type to markup</typeparam>
        /// <param name="markup">Markup</param>
        /// <returns><see cref="FormattingProfile"/></returns>
        public static FormattingProfile AddTypeStyle<T>(this FormattingProfile formattingProfile,
            string markup)
        {
            return formattingProfile.ConfigureMultiTypeRenderers(opt => opt.AddTypeStyle<T>(markup));
        }
        
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
            return formattingProfile.ConfigureMultiTypeRenderers(opt => opt.AddTypeStyle(type, markup));
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
            return formattingProfile.ConfigureMultiTypeRenderers(opt => opt.AddTypeStyle(types, markup));
        }

        /// <summary>
        /// Clears all type style formatting.
        /// </summary>
        /// <param name="formattingProfile">Formatting profile</param>
        /// <returns><see cref="FormattingProfile"/></returns>
        public static FormattingProfile ClearTypeStyles(this FormattingProfile formattingProfile)
        {
            return formattingProfile.ConfigureMultiTypeRenderers(opt => opt.ClearTypeStyles());
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
            return formattingProfile.ConfigureMultiTypeRenderers(opt => opt.AddValueStyle(value, markup));
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
            return formattingProfile.ConfigureMultiTypeRenderers(opt => opt.AddValueStyles(values, markup));
        }
        
        /// <summary>
        /// Clears all value styles.
        /// </summary>
        /// <param name="formattingProfile">Formatting profile</param>
        /// <returns><see cref="FormattingProfile"/></returns>
        public static FormattingProfile ClearValueStyles(this FormattingProfile formattingProfile)
        {
            return formattingProfile.ConfigureMultiTypeRenderers(opt => opt.ClearValueStyles());
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
            return formattingProfile.ConfigureMultiTypeRenderers(opt => opt.AddTypeFormatter(type, formatter));
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
            return formattingProfile.ConfigureMultiTypeRenderers(opt => opt.AddTypeFormatters(types, formatter));
        }

        /// <summary>
        /// Adds a function that formats the output of a specific value type.
        /// </summary>
        /// <param name="formattingProfile">Formatting profile.</param>
        /// <typeparam name="T">The value type to format.</typeparam>
        /// <param name="formatter">A function that returns a string representation of the given value.</param>
        /// <returns><see cref="FormattingProfile"/></returns>
        public static FormattingProfile AddTypeFormatter<T>(this FormattingProfile formattingProfile,
            Func<T?, string?> formatter)
        {
            return formattingProfile.ConfigureMultiTypeRenderers(opt => opt.AddTypeFormatter(formatter));
        }
        

        /// <summary>
        /// Clears all type value formatting functions.
        /// </summary>
        /// <param name="formattingProfile">Formatting profile</param>
        /// <returns><see cref="FormattingProfile"/></returns>
        public static FormattingProfile ClearValueFormatters(this FormattingProfile formattingProfile)
        {
            return formattingProfile.ConfigureMultiTypeRenderers(opt => opt.ClearValueFormatters());
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
        public static TOptions? GetRendererOptions<TOptions>(this FormattingProfile formattingProfile)
            where TOptions : class
        {
            return formattingProfile.RendererOptions.GetValueOrDefault(typeof(TOptions)) as TOptions;
        }

        private static FormattingProfile ConfigureMultiTypeRenderers(this FormattingProfile formattingProfile,
            Action<MultiTypeRenderingOptions> configure)
        {
            formattingProfile.ConfigureRenderer<KeyedPropertyValueRenderer.Options>(configure);
            formattingProfile.ConfigureRenderer<MessageTemplateRenderer.Options>(configure);
            return formattingProfile;
        }
    }
}