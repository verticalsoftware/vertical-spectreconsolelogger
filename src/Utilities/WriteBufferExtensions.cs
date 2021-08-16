using System;
using Spectre.Console;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Infrastructure;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Output;

namespace Vertical.SpectreLogger.Utilities
{
    public static class WriteBufferExtensions
    {
        public static bool AtMargin(this IWriteBuffer buffer) => buffer.CharPosition == buffer.Margin;

        public static void WriteLine(this IWriteBuffer buffer) => buffer.Write(Environment.NewLine);

        /// <summary>
        /// Writes to the buffer.
        /// </summary>
        /// <param name="buffer">Buffer instance</param>
        /// <param name="profile">Formatting profile</param>
        /// <param name="templateContext">Template context or null</param>
        /// <param name="formatter">Custom formatter or null</param>
        /// <param name="value">Value</param>
        /// <typeparam name="T">Value type</typeparam>
        /// <returns>The string that was written</returns>
        public static void Write<T>(this IWriteBuffer buffer, 
            FormattingProfile profile,
            TemplateContext? templateContext,
            IFormatter? formatter,
            T value) where T : notnull
        {
            var type = typeof(T);
            
            var markup = profile.ValueStyles.GetValueOrDefault(value)
                         ??
                         profile.TypeStyles.GetValueOrDefault(type);

            if (markup != null)
            {
                buffer.Write($"[{markup}]");
            }

            var formattedValue = FormatValue(profile, templateContext, formatter, value);
            
            buffer.Write(formattedValue.EscapeMarkup());

            if (markup != null)
            {
                buffer.Write("[/]");
            }
        }

        private static string FormatValue<T>(FormattingProfile profile, 
            TemplateContext? templateContext, 
            IFormatter? formatter, 
            T value) 
            where T : notnull
        {
            string? formattedValue;

            for (;;)
            {
                if ((formattedValue = profile.ValueFormatters.GetValueOrDefault(value)?.Invoke(value)) != null)
                    break;

                if ((formattedValue = profile.TypeFormatters.GetValueOrDefault(typeof(T))?.Invoke(value)) != null)
                    break;

                var format = templateContext?.Format;

                if (format != null && ((formattedValue = formatter?.Format(format, value)) != null))
                    break;

                formattedValue = value.ToString();
                break;
            }

            if (templateContext?.Width.HasValue == true)
            {
                var formatString = $"{{0,{templateContext.Width}}}";
                formattedValue = string.Format(formatString, formattedValue);
            }

            return formattedValue;
        }
    }
}