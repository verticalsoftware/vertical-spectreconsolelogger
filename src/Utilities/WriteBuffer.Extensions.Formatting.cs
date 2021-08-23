using System.Collections.Generic;
using Spectre.Console;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Formatting;
using Vertical.SpectreLogger.Infrastructure;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.Types;

namespace Vertical.SpectreLogger.Utilities
{
    public static partial class WriteBufferExtensions
    {
        /// <summary>
        /// Writes to the buffer.
        /// </summary>
        /// <param name="buffer">Buffer instance</param>
        /// <param name="profile">Formatting profile</param>
        /// <param name="templateContext">Template context or null</param>
        /// <param name="formatter">Custom formatter or null</param>
        /// <param name="value">Value</param>
        /// <param name="options">Formatting options to apply</param>
        /// <typeparam name="T">Value type</typeparam>
        /// <returns>The string that was written</returns>
        public static string CaptureFormattedValue<T>(this IWriteBuffer buffer, 
            T value,
            FormattingProfile profile,
            TemplateContext? templateContext = null,
            IFormatter? formatter = null,
            FormattingOptions options = FormattingOptions.All) 
            where T : notnull
        {
            var position = buffer.Length;
            var markup = GetMarkup(profile, value, options);

            if (markup != null)
            {
                buffer.Write($"[{markup}]");
            }

            var formattedValue = GetFormattedValue(profile, templateContext, formatter, value, options);
            
            buffer.Write(formattedValue.EscapeMarkup());

            if (markup != null)
            {
                buffer.Write("[/]");
            }

            var writeLength = buffer.Length - position;
            
            return writeLength > 0 ? buffer.ToString(position, writeLength) : string.Empty;
        }

        /// <summary>
        /// Writes values that are to be structured within a template string.
        /// </summary>
        /// <param name="buffer">Write buffer</param>
        /// <param name="template">Template string</param>
        /// <param name="logValues">Collection of formatted log values</param>
        /// <param name="profile">Formatting profile</param>
        /// <param name="options">Options</param>
        public static void WriteTemplate(
            this IWriteBuffer buffer,
            string? template,
            IReadOnlyList<KeyValuePair<string, object>> logValues,
            FormattingProfile profile,
            FormattingOptions options = FormattingOptions.All)
        {
            foreach (var templateSpan in TemplateParser.Split(template!))
            {
                if (templateSpan.IsTemplate)
                {
                    var valueKey = templateSpan.TemplateKey;

                    if (!logValues.TryGetFormattedLogValue(valueKey, out var logValue))
                    {
                        buffer.Write(templateSpan.Value);
                        continue;
                    }

                    buffer.WriteFormattedValue(
                        logValue ?? NullLogValue.Default, 
                        profile,
                        options: options);

                    continue;
                }

                buffer.Write(templateSpan.Value);
            }
        }

        /// <summary>
        /// Writes a state value.
        /// </summary>
        /// <param name="buffer">Write buffer</param>
        /// <param name="profile">Formatting profile</param>
        /// <param name="state">State object to write</param>
        /// <typeparam name="TState"></typeparam>
        public static void WriteStateValue<TState>(
            this IWriteBuffer buffer, 
            FormattingProfile profile,
            TState state)
        {
            if (state is not IReadOnlyList<KeyValuePair<string, object>> logValues)
                return;

            if (!logValues.TryGetFormattedLogValue<string>("{OriginalFormat}", out var template))
            {
                buffer.WriteFormattedValue((object?)state ?? NullLogValue.Default, profile);
                return;
            }

            buffer.WriteTemplate(template, logValues, profile);
        }
        
        /// <summary>
        /// Writes to the buffer.
        /// </summary>
        /// <param name="buffer">Buffer instance</param>
        /// <param name="profile">Formatting profile</param>
        /// <param name="templateContext">Template context or null</param>
        /// <param name="formatter">Custom formatter or null</param>
        /// <param name="value">Value</param>
        /// <param name="options">Formatting options to apply</param>
        /// <typeparam name="T">Value type</typeparam>
        /// <returns>The string that was written</returns>
        public static void WriteFormattedValue<T>(this IWriteBuffer buffer, 
            T value,
            FormattingProfile profile,
            TemplateContext? templateContext = null,
            IFormatter? formatter = null,
            FormattingOptions options = FormattingOptions.All) 
            where T : notnull
        {
            var markup = GetMarkup(profile, value, options);

            if (markup != null)
            {
                buffer.Write($"[{markup}]");
            }

            var formattedValue = GetFormattedValue(profile, templateContext, formatter, value, options);
            
            buffer.Write(formattedValue.EscapeMarkup());

            if (markup != null)
            {
                buffer.Write("[/]");
            }
        }

        private static string? GetMarkup<T>(FormattingProfile profile, T value, FormattingOptions options) where T : notnull
        {
            return (options.ApplyValueStyle() ? profile.ValueStyles.GetValueOrDefault(value) : null)
                   ??
                   (options.ApplyTypeStyle() ? profile.TypeStyles.GetValueOrDefault(typeof(T)) : null)
                   ??
                   (options.ApplyDefaultStyle() ? profile.DefaultLogValueStyle : null);
        }

        private static string GetFormattedValue<T>(FormattingProfile profile, 
            TemplateContext? templateContext, 
            IFormatter? formatter,
            T value,
            FormattingOptions options) 
            where T : notnull
        {
            string? formattedValue;
            var templateFormat = templateContext?.Format ?? string.Empty;

            for (;;)
            {
                if (options.ApplyValueFormat() && (formattedValue = profile.ValueFormatters.GetValueOrDefault(value)) != null)
                    break;

                if (options.ApplyTypeFormat() && (formattedValue = profile.TypeFormatters.GetValueOrDefault(typeof(T))?.Format(templateFormat, value)) != null)
                    break;

                var format = templateContext?.Format;

                if (options.ApplyTemplateFormat() && format != null)
                {
                    if ((formattedValue = formatter?.Format(format, value)) != null)
                        break;

                    var compositeFormat = $"{{0:{format}}}";
                    formattedValue = string.Format(compositeFormat, value);
                    break;
                }

                if (options.ApplyDefaultFormat() && (formattedValue = profile.DefaultLogValueFormatter?.Format(templateFormat, value)) != null)
                    break;

                formattedValue = value.ToString();
                
                break;
            }

            if (options.ApplyTemplateWidth() && templateContext?.Width.HasValue == true)
            {
                var formatString = $"{{0,{templateContext.Width}}}";
                formattedValue = string.Format(formatString, formattedValue);
            }

            return formattedValue!;
        }
    }
}