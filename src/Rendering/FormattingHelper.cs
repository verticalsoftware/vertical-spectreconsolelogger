using Spectre.Console;
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
        public static void RenderLogValue(FormattingProfile profile,
            IWriteBuffer buffer,
            object? obj)
        {
            var formattedValue = FormattedValue.CreateFromProfile(profile, obj);

            // Nothing to render
            if (!formattedValue.HasValue)
                return;
            
            buffer.Write(formattedValue.Value!, formattedValue.Markup);
        }
    }
}