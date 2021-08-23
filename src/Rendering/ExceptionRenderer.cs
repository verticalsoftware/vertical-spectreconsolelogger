using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Output;

namespace Vertical.SpectreLogger.Rendering
{
    [Template("{Exception}")]
    public class ExceptionRenderer : ITemplateRenderer
    {
        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventInfo eventInfo)
        {
            if (eventInfo.Exception == null)
                return;
        }
    }
}