using Spectre.Console;
using Vertical.SpectreLogger.Internal;
using Vertical.SpectreLogger.MatchableTypes;
using Vertical.SpectreLogger.Output;

namespace Vertical.SpectreLogger.Rendering
{
    /// <summary>
    /// Renders the category name.
    /// </summary>
    [Template("{CategoryName}")]
    public class CategoryNameRenderer : ITemplateRenderer
    {
        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, ref LogEventInfo eventInfo)
        {
            if (string.IsNullOrWhiteSpace(eventInfo.CategoryName))
                return;

            var categoryName = eventInfo.CategoryName;

            var profile = eventInfo.FormattingProfile;
            var formatted = profile.ValueFormatters.GetValueOrDefault(typeof(CategoryName))?
                .Invoke(categoryName) ?? categoryName;

            buffer.Write(formatted.EscapeMarkup());
        }
    }
}