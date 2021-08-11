using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.Templates;

namespace Vertical.SpectreLogger.Rendering
{
    /// <summary>
    /// Controls offsets of the margin.
    /// </summary>
    public class MarginControlRenderer : ITemplateRenderer
    {
        private readonly TemplateContext _templateContext;

        [TemplateProvider] 
        public static readonly Template Template = new()
        {
            CustomPattern = $"{{Margin@(?<{CaptureGroups.Margin}>-?\\d+)(?<{CaptureGroups.MarginSet}>!)?}}"
        };

        public MarginControlRenderer(TemplateContext templateContext)
        {
            _templateContext = templateContext;
        }
        
        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventInfo eventInfo)
        {
            buffer.SetMargin(_templateContext);
        }
    }
}