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
            
            var rendered = eventInfo.FormattingProfile.ValueFormatters.TryGetValue(typeof(CategoryName), out var function)
                ? function(categoryName)
                : categoryName;

            if (rendered == null)
                return;

            buffer.Append(eventInfo.FormattingProfile, rendered);
        }
    }
}