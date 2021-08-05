using Vertical.SpectreLogger.Internal;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.PseudoTypes;

namespace Vertical.SpectreLogger.Rendering
{
    /// <summary>
    /// Helpers for formatting.
    /// </summary>
    public static class FormattingHelper
    {
        /// <summary>
        /// Renders a log value by applying markup and transformations.
        /// </summary>
        /// <param name="profile">Formatting profile</param>
        /// <param name="buffer">Buffer to write to</param>
        /// <param name="obj">Value to render</param>
        public static void RenderFormattedValue(FormattingProfile profile,
            IWriteBuffer buffer,
            object? obj)
        {
            var formattedValue = CreateFormattedValue(profile, obj);

            // Nothing to render
            if (!formattedValue.HasValue)
                return;
            
            buffer.Write(formattedValue.Value!, formattedValue.Markup);
        }

        /// <summary>
        /// Formats the value using a type formatter in the profile.
        /// </summary>
        /// <param name="profile">Formatting profile</param>
        /// <param name="obj">Value to format</param>
        /// <returns>The nullable formatted value
        /// </returns>
        /// <remarks>
        /// This method uses the default type formatter if a specific one is not found.
        /// </remarks>
        public static string? GetProfileFormatOrDefault(FormattingProfile profile, object? obj)
        {
            var formatter =
                profile.TypeFormatters.GetValueOrDefault(obj?.GetType() ?? typeof(NullValue))
                ??
                profile.DefaultTypeFormatter;

            return formatter?.Invoke(obj) ?? obj?.ToString();
        }

        /// <summary>
        /// Formats the value using a type formatter in the profile.
        /// </summary>
        /// <param name="profile">Formatting profile</param>
        /// <param name="obj">Value to format</param>
        /// <returns>The nullable formatted value
        /// </returns>
        public static string? GetProfileFormat(FormattingProfile profile, object? obj)
        {
            return profile.TypeFormatters.GetValueOrDefault(obj?.GetType() ?? typeof(NullValue))
                ?.Invoke(obj);
        }
        
        /// <summary>
        /// Formats the string using composite formatting.
        /// </summary>
        /// <param name="value">Value to format.</param>
        /// <param name="alignment">Alignment</param>
        /// <param name="format">Format</param>
        /// <returns></returns>
        public static string? GetCompositeFormat(string value, string? alignment, string? format)
        {
            return alignment == null && format == null
                ? value
                : string.Format($"{{0{alignment ?? string.Empty}{format ?? string.Empty}}}", value);
        }

        /// <summary>
        /// Obtains the markup to use for a particular value.
        /// </summary>
        /// <param name="profile">Formatting profile</param>
        /// <param name="obj">Value</param>
        /// <returns>Markup to apply or null.</returns>
        public static string? GetProfileMarkupOrDefault(FormattingProfile profile, object? obj)
        {
            switch (obj)
            {
                case null:
                    return profile.TypeStyles.GetValueOrDefault(typeof(NullValue), profile.DefaultTypeStyle);
                
                default:
                    var type = obj.GetType();

                    return profile.ValueStyles.GetValueOrDefault((type, obj!))
                           ?? profile.TypeStyles.GetValueOrDefault(type, profile.DefaultTypeStyle);
            }
        }

        /// <summary>
        /// Creates a formatted value.
        /// </summary>
        /// <param name="profile">Formatting profile.</param>
        /// <param name="obj">Value to format and markup.</param>
        /// <returns><see cref="FormattedValue"/></returns>
        public static FormattedValue CreateFormattedValue(FormattingProfile profile, object? obj)
        {
            var value = GetProfileFormatOrDefault(profile, obj);
            var markup = GetProfileMarkupOrDefault(profile, obj);

            return new FormattedValue(value, markup);
        }
    }
}