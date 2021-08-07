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

        public TimestampRenderer(Match matchContext)
        {
            _alignment = matchContext.Groups[1].Value;
            _format = matchContext.Groups[2].Value;
        }

        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventInfo eventInfo)
        {
            var options = eventInfo.FormattingProfile.GetRendererOptions<TimestampRenderingOptions>();
            var formattedValue =
                options?.Formatter?.Invoke(eventInfo.Timestamp)
                ??
                FormattingHelper.GetCompositeFormat(eventInfo.Timestamp, _alignment, _format);
            
            buffer.Write(formattedValue, options?.Style);
        }
    }
}