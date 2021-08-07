using System.Collections.Concurrent;
using System.Linq;
using System.Text.RegularExpressions;
using Spectre.Console;
using Vertical.SpectreLogger.Internal;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.PseudoTypes;

namespace Vertical.SpectreLogger.Rendering
{
    /// <summary>
    /// Renders the category name.
    /// </summary>
    [Template(MyTemplate)]
    public class CategoryNameRenderer : ITemplateRenderer
    {
        private const string MyTemplate = @"{CategoryName(,-?\d+)?(:S(\d+))?}";
        private readonly string _alignment;
        private readonly int? _segments;
        private readonly ConcurrentDictionary<string, FormattedValue> _cachedEntries = new();

        public CategoryNameRenderer(Match matchContext)
        {
            _alignment = matchContext.Groups[1].Value;
            _segments = matchContext.Groups[3].Success
                ? int.Parse(matchContext.Groups[3].Value)
                : null;
        }

        public class Options : TypeRenderingOptions<string>
        {
        }
        
        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventInfo eventInfo)
        {
            if (string.IsNullOrWhiteSpace(eventInfo.CategoryName))
                return;

            buffer.Write(GetFormattedValue(eventInfo));
        }

        private FormattedValue GetFormattedValue(in LogEventInfo eventInfo)
        {
            var categoryName = eventInfo.CategoryName;
            
            if (_cachedEntries.TryGetValue(categoryName, out var formattedValue))
            {
                return formattedValue;
            }
            
            if (_segments.HasValue)
            {
                var split = categoryName.Split('.');
                
                categoryName = _segments.Value < split.Length
                    ? string.Join(".", split.Reverse().Take(_segments.Value).Reverse())
                    : categoryName;
            }

            var profile = eventInfo.FormattingProfile;
            var rendererOptions = profile.GetRendererOptions<Options>();
            var profileFormat = rendererOptions?.Formatter?.Invoke(categoryName) ?? categoryName;
            var compositeFormat = FormattingHelper.GetCompositeFormat(profileFormat, _alignment, null);
            var markup = rendererOptions?.Style;
            
            formattedValue = new FormattedValue(compositeFormat.EscapeMarkup(), markup);

            _cachedEntries.TryAdd(categoryName, formattedValue);

            return formattedValue;
        }
    }
}