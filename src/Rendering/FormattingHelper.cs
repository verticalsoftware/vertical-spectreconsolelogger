using Vertical.SpectreLogger.Internal;
using Vertical.SpectreLogger.MatchableTypes;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Output;

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
        public static string? FormatValue(FormattingProfile profile, object? obj)
        {
            var formatter =
                profile.TypeFormatters.GetValueOrDefault(obj?.GetType() ?? typeof(Null))
                ??
                profile.DefaultTypeFormatter;

            return formatter?.Invoke(obj) ?? obj?.ToString();
        }

        /// <summary>
        /// Obtains the markup to use for a particular value.
        /// </summary>
        /// <param name="profile">Formatting profile</param>
        /// <param name="obj">Value</param>
        /// <returns>Markup to apply or null.</returns>
        public static string? FormatMarkup(FormattingProfile profile, object? obj)
        {
            return profile.TypeStyles.GetValueOrDefault(obj?.GetType() ?? typeof(Null))
                   ?? profile.DefaultTypeStyle;
        }

        /// <summary>
        /// Creates a formatted value.
        /// </summary>
        /// <param name="profile">Formatting profile.</param>
        /// <param name="obj">Value to format and markup.</param>
        /// <returns><see cref="FormattedValue"/></returns>
        public static FormattedValue CreateFormattedValue(FormattingProfile profile, object? obj)
        {
            var value = FormatValue(profile, obj);
            var markup = FormatMarkup(profile, obj);

            return new FormattedValue(value, markup);
        }
    }
}