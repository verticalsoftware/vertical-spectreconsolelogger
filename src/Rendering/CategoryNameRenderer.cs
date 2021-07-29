using Vertical.SpectreLogger.MatchableTypes;
using Vertical.SpectreLogger.Output;

namespace Vertical.SpectreLogger.Rendering
{
    /// <summary>
    /// Renders the category name.
    /// </summary>
    public class CategoryNameRenderer : ITemplateRenderer
    {
        /// <inheritdoc />
        public string Template => "{CategoryName}";

        /// <inheritdoc />
        public void Format(IWriteBuffer buffer, ref LogEventInfo eventInfo)
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