using System;
using Spectre.Console;
using Vertical.SpectreLogger.Internal;
using Vertical.SpectreLogger.Options;

namespace Vertical.SpectreLogger.Rendering
{
    /// <summary>
    /// Helpers for formatting.
    /// </summary>
    public static class FormattingHelper
    {
        /// <summary>
        /// Formats the string using composite formatting.
        /// </summary>
        /// <param name="value">Value to format.</param>
        /// <param name="alignment">Alignment</param>
        /// <param name="format">Format</param>
        /// <returns>The formatted string.</returns>
        public static string GetCompositeFormat(object value, string? alignment = null, string? format = null)
        {
            return alignment == null && format == null
                ? value.ToString()!
                : string.Format($"{{0{alignment ?? string.Empty}{format ?? string.Empty}}}", value);
        }

        public static string FormatValue(MultiTypeRenderingOptions? options,
            object value,
            Type type,
            string? width = null,
            string? format = null)
        {
            if (options == null)
            {
                return value.ToString().EscapeMarkup();
            }

            var compositeFormat = $"{width}{format}";
            
            if (!string.IsNullOrWhiteSpace(compositeFormat))
            {
                var formatString = $"{{0{compositeFormat}}}";
                return string.Format(formatString, value);
            }
            
            var formatted =
                options.TypeFormatters.GetValueOrDefault(type)?.Invoke(value)
                ??
                options.DefaultTypeFormatter?.Invoke(value)
                ??
                value.ToString();

            return formatted.EscapeMarkup();
        }

        public static string? MarkupValue(MultiTypeRenderingOptions? options, object value, Type type)
        {
            return
                options?.ValueStyles.GetValueOrDefault((type, value!))
                ??
                options?.TypeStyles.GetValueOrDefault(type)
                ??
                options?.DefaultTypeStyle;
        }
    }
}