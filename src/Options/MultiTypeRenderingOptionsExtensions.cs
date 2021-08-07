using System;
using System.Collections.Generic;

namespace Vertical.SpectreLogger.Options
{
    public static class MultiTypeRenderingOptionsExtensions
    {
        /// <summary>
        /// Adds markup that styles the rendering of specific object types.
        /// </summary>
        /// <param name="options">˙</param>
        /// <typeparam name="T">Type to markup</typeparam>
        /// <param name="markup">Markup</param>
        /// <returns><see cref="MultiTypeRenderingOptions"/></returns>
        public static MultiTypeRenderingOptions AddTypeStyle<T>(this MultiTypeRenderingOptions options,
            string markup) => options.AddTypeStyle(typeof(T), markup);
        
        /// <summary>
        /// Adds markup that styles the rendering of specific object types.
        /// </summary>
        /// <param name="options">Options instance</param>
        /// <param name="type">Type to markup</param>
        /// <param name="markup">Markup</param>
        /// <returns><see cref="MultiTypeRenderingOptions"/></returns>
        public static MultiTypeRenderingOptions AddTypeStyle(this MultiTypeRenderingOptions options, 
            Type type, 
            string markup)
        {
            options.TypeStyles[type] = markup;
            return options;
        }

        /// <summary>
        /// Adds markup that styles the rendering of specific object types.
        /// </summary>
        /// <param name="options">Options instance</param>
        /// <param name="types">Type to markup</param>
        /// <param name="markup">Markup</param>
        /// <returns><see cref="MultiTypeRenderingOptions"/></returns>
        public static MultiTypeRenderingOptions AddTypeStyle(this MultiTypeRenderingOptions options,
            IEnumerable<Type> types,
            string markup)
        {
            foreach (var type in types)
            {
                options.AddTypeStyle(type, markup);
            }

            return options;
        }

        /// <summary>
        /// Clears all type style formatting.
        /// </summary>
        /// <param name="options">Options instance</param>
        /// <returns><see cref="MultiTypeRenderingOptions"/></returns>
        public static MultiTypeRenderingOptions ClearTypeStyles(this MultiTypeRenderingOptions options)
        {
            options.TypeStyles.Clear();
            return options;
        }

        /// <summary>
        /// Adds a style for a specific value.
        /// </summary>
        /// <param name="options">Options instance</param>
        /// <param name="value">The value the style is applied to</param>
        /// <param name="markup">The markup to apply before rendering the value</param>
        /// <typeparam name="T">The value type</typeparam>
        /// <returns><see cref="MultiTypeRenderingOptions"/></returns>
        public static MultiTypeRenderingOptions AddValueStyle<T>(this MultiTypeRenderingOptions options,
            T value,
            string markup) where T : notnull
        {
            options.ValueStyles[(typeof(T), value)] = markup;
            return options;
        }

        /// <summary>
        /// Adds a style for specific values.
        /// </summary>
        /// <param name="options">Options instance</param>
        /// <param name="values">The value the style is applied to</param>
        /// <param name="markup">The markup to apply before rendering any of the provided values</param>
        /// <returns><see cref="MultiTypeRenderingOptions"/></returns>
        /// <exception cref="ArgumentException"><paramref name="values"/> contains a null value.</exception>
        public static MultiTypeRenderingOptions AddValueStyles(this MultiTypeRenderingOptions options,
            IEnumerable<object> values,
            string markup)
        {
            var valueStyles = options.ValueStyles;
            
            foreach (var obj in values)
            {
                var type = obj?.GetType() ?? throw new ArgumentException("Null values are not allowed.", nameof(values));
                valueStyles[(type, obj)] = markup;
            }

            return options;
        }
        
        /// <summary>
        /// Clears all value styles.
        /// </summary>
        /// <param name="options">Options instance</param>
        /// <returns><see cref="MultiTypeRenderingOptions"/></returns>
        public static MultiTypeRenderingOptions ClearValueStyles(this MultiTypeRenderingOptions options)
        {
            options.ValueStyles.Clear();
            return options;
        }

        /// <summary>
        /// Adds a function that formats the output of a specific value type.
        /// </summary>
        /// <param name="options">Options instance.</param>
        /// <param name="type">The value type to format.</param>
        /// <param name="formatter">A function that returns a string representation of the given value.</param>
        /// <returns><see cref="MultiTypeRenderingOptions"/></returns>
        public static MultiTypeRenderingOptions AddTypeFormatter(this MultiTypeRenderingOptions options,
            Type type,
            Func<object?, string?> formatter)
        {
            options.TypeFormatters[type] = formatter;
            return options;
        }

        /// <summary>
        /// Adds a function that formats the output of specific value types.
        /// </summary>
        /// <param name="options">Options instance.</param>
        /// <param name="types">The value type to format.</param>
        /// <param name="formatter">A function that returns a string representation of the given value.</param>
        /// <returns><see cref="MultiTypeRenderingOptions"/></returns>
        public static MultiTypeRenderingOptions AddTypeFormatters(this MultiTypeRenderingOptions options,
            IEnumerable<Type> types,
            Func<object?, string?> formatter)
        {
            foreach (var type in types)
            {
                options.AddTypeFormatter(type, formatter);
            }

            return options;
        }

        /// <summary>
        /// Adds a function that formats the output of a specific value type.
        /// </summary>
        /// <param name="options">Options instance.</param>
        /// <typeparam name="T">The value type to format.</typeparam>
        /// <param name="formatter">A function that returns a string representation of the given value.</param>
        /// <returns><see cref="MultiTypeRenderingOptions"/></returns>
        public static MultiTypeRenderingOptions AddTypeFormatter<T>(this MultiTypeRenderingOptions options,
            Func<T?, string?> formatter) => options.AddTypeFormatter(typeof(T), obj => formatter((T?)obj));
        

        /// <summary>
        /// Clears all type value formatting functions.
        /// </summary>
        /// <param name="options">Options instance</param>
        /// <returns><see cref="MultiTypeRenderingOptions"/></returns>
        public static MultiTypeRenderingOptions ClearValueFormatters(this MultiTypeRenderingOptions options)
        {
            options.TypeFormatters.Clear();
            return options;
        }
    }
}