using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.Templates;

namespace Vertical.SpectreLogger.Rendering
{
    public class LogLevelRenderer : ITemplateRenderer
    {
        private readonly TemplateContext _templateContext;

        public class Options : TypeRenderingOptions<LogLevel>
        {
        }

        [TemplateProvider]
        public static readonly Template Template = new()
        {
            RendererKey = "LogLevel",
            FieldWidthFormatting = true
        };

        private readonly ConcurrentDictionary<LogLevel, FormattedValue> _cachedFormats = new();

        public LogLevelRenderer(TemplateContext templateContext)
        {
            _templateContext = templateContext;
        }
        
        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventInfo eventInfo)
        {
            if (!_cachedFormats.TryGetValue(eventInfo.LogLevel, out var formattedValue))
            {
                var profile = eventInfo.FormattingProfile;
                var rendererOptions = profile.GetRendererOptions<Options>();
                var profileFormat = rendererOptions?.Formatter?.Invoke(eventInfo.LogLevel) ?? eventInfo.LogLevel.ToString();
                var compositeFormat = FormattingHelper.GetCompositeFormat(profileFormat, _templateContext.FieldWidth);
                var markup = rendererOptions?.Style;

                formattedValue = new FormattedValue(compositeFormat.EscapeMarkup(), markup);

                _cachedFormats.TryAdd(eventInfo.LogLevel, formattedValue);
            }
            
            buffer.Write(formattedValue);
        }
    }
}