using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.Utilities;

namespace Vertical.SpectreLogger.Rendering
{
    [Template(@"{LogLevel" + TemplatePatterns.WidthCapturePattern + "}")]
    public class LogLevelRenderer : ITemplateRenderer
    {
        private readonly TemplateContext _templateContext;

        public LogLevelRenderer(TemplateContext templateContext)
        {
            _templateContext = templateContext;
        }
        
        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventInfo eventInfo)
        {
            buffer.Write(
                eventInfo.FormattingProfile,
                _templateContext,
                null,
                eventInfo.LogLevel);
        }
    }
}