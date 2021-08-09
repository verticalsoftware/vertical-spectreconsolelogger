using System;
using System.Text.RegularExpressions;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.Templates;

namespace Vertical.SpectreLogger.Rendering
{
    [Template(MyTemplate)]
    public class TimestampRenderer : ITemplateRenderer
    {
        private const string MyTemplate = @"{Timestamp(,-?\d+)?(:[^}]+)?}";
        
        private readonly string _alignment;
        private readonly string _format;

        public class Options : TypeRenderingOptions<DateTimeOffset>
        {
        }

        public TimestampRenderer(Match matchContext)
        {
            _alignment = matchContext.Groups[1].Value;
            _format = matchContext.Groups[2].Value;
        }

        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventInfo eventInfo)
        {
            var options = eventInfo.FormattingProfile.GetRendererOptions<Options>();
            var value = eventInfo.Timestamp;
            var formattedValue =
                options?.Formatter?.Invoke(value)
                ??
                FormattingHelper.GetCompositeFormat(value, _alignment, _format);
            
            buffer.Write(formattedValue, options?.Style);
        }
    }
}