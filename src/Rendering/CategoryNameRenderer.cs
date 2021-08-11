using System.Collections.Concurrent;
using System.Linq;
using Spectre.Console;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.Templates;

namespace Vertical.SpectreLogger.Rendering
{
    /// <summary>
    /// Renders the category name.
    /// </summary>
    public class CategoryNameRenderer : ITemplateRenderer
    {
        private readonly ConcurrentDictionary<string, FormattedValue> _cachedEntries = new();
        private readonly TemplateContext _templateContext;
        private readonly int? _segments;

        [TemplateProvider]
        public static readonly Template Template = new()
        {
            RendererKey = "CategoryName",
            FieldWidthFormatting = true,
            CustomFormat = "(:S(?<segments>\\d+))?"
        };

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="templateContext">Template context</param>
        public CategoryNameRenderer(TemplateContext templateContext)
        {
            _templateContext = templateContext;
            _segments = templateContext.MatchContext.Groups["segments"].Success
                ? int.Parse(templateContext.MatchContext.Groups["segments"].Value)
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
            var compositeFormat = FormattingHelper.GetCompositeFormat(profileFormat, _templateContext.FieldWidth);
            var markup = rendererOptions?.Style;
            
            formattedValue = new FormattedValue(compositeFormat.EscapeMarkup(), markup);

            _cachedEntries.TryAdd(categoryName, formattedValue);

            return formattedValue;
        }
    }
}