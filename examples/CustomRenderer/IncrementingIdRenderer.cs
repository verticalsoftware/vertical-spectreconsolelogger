using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.Templates;

namespace CustomRenderer
{
    public class IncrementingIdRenderer : ITemplateRenderer
    {
        [Template]
        public static readonly string Template = TemplatePatternBuilder
            .ForKey("IncrementingId")
            .AddAlignment()
            .AddFormatting()
            .Build();

        private readonly TemplateSegment _templateSegment;

        public IncrementingIdRenderer(TemplateSegment templateSegment) => _templateSegment = templateSegment;
        
        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventContext context)
        {
            buffer.WriteLogValue(
                context.Profile,
                _templateSegment,
                IncrementingId.Create());
        }
    }
}