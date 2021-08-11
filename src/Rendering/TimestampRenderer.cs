using System;
using System.Text.RegularExpressions;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.Templates;

namespace Vertical.SpectreLogger.Rendering
{
    public class TimestampRenderer : ITemplateRenderer
    {
        private readonly TemplateContext _templateContext;

        [TemplateProvider]
        public static readonly Template Template = new()
        {
            RendererKey = "Timestamp",
            FieldWidthFormatting = true,
            CompositeFormatting = true
        };

        public class Options : TypeRenderingOptions<DateTimeOffset>
        {
        }

        public TimestampRenderer(TemplateContext templateContext)
        {
            _templateContext = templateContext;
        }

        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventInfo eventInfo)
        {
            var options = eventInfo.FormattingProfile.GetRendererOptions<Options>();
            var value = eventInfo.Timestamp;
            var formattedValue =
                options?.Formatter?.Invoke(value)
                ??
                FormattingHelper.GetCompositeFormat(value, 
                    _templateContext.FieldWidth, 
                    _templateContext.CompositeFormat);
            
            buffer.Write(formattedValue, options?.Style);
        }
    }
}