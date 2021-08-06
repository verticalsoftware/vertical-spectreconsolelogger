using System.Text.RegularExpressions;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Output;

namespace Vertical.SpectreLogger.Rendering
{
    [Template(MyTemplate)]
    public class TimestampRenderer : ITemplateRenderer
    {
        private const string MyTemplate = @"{Timestamp(,-?\d+)?(:[^}]+)?}";
        
        private readonly string _alignment;
        private readonly string _format;

        public TimestampRenderer(string templateContext)
        {
            var match = Regex.Match(templateContext, MyTemplate);

            _alignment = match.Groups[1].Value;
            _format = match.Groups[2].Value;
        }

        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventInfo eventInfo)
        {
            var options = eventInfo.FormattingProfile.GetRenderingOptions<TimestampRenderingOptions>();
            var formattedValue =
                options?.Formatter?.Invoke(eventInfo.Timestamp)
                ??
                FormattingHelper.GetCompositeFormat(eventInfo.Timestamp, _alignment, _format);
            
            buffer.Write(formattedValue, options?.Style);
        }
    }
}