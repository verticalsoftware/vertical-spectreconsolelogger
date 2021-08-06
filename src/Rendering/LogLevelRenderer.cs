using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Output;

namespace Vertical.SpectreLogger.Rendering
{
    [Template(MyTemplate)]
    public class LogLevelRenderer : ITemplateRenderer
    {
        private const string MyTemplate = @"{LogLevel(,-?\d+)?}";
        
        private readonly ConcurrentDictionary<LogLevel, FormattedValue> _cachedFormats = new();
        private readonly string? _format;

        public LogLevelRenderer(string templateContext)
        {
            var match = Regex.Match(templateContext, MyTemplate);

            if (match.Groups[1].Success)
            {
                _format = match.Groups[1].Value;
            }
        }
        
        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventInfo eventInfo)
        {
            if (!_cachedFormats.TryGetValue(eventInfo.LogLevel, out var formattedValue))
            {
                var profile = eventInfo.FormattingProfile;
                var rendererOptions = profile.GetRenderingOptions<LogLevelRenderingOptions>();
                var profileFormat = rendererOptions?.Formatter?.Invoke(eventInfo.LogLevel) ?? eventInfo.LogLevel.ToString();
                var compositeFormat = FormattingHelper.GetCompositeFormat(profileFormat, null, _format);
                var markup = rendererOptions?.Style;

                formattedValue = new FormattedValue(compositeFormat.EscapeMarkup(), markup);

                _cachedFormats.TryAdd(eventInfo.LogLevel, formattedValue);
            }
            
            buffer.Write(formattedValue);
        }
    }
}