using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.Utilities;

namespace Vertical.SpectreLogger.Rendering
{
    [Template("{Message}")]
    public class MessageRenderer : ITemplateRenderer
    {
        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventInfo eventInfo)
        {
            buffer.WriteStateValue(eventInfo.FormattingProfile, eventInfo.State);
        }
    }
}