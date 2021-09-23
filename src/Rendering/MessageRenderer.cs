using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.Templates;

namespace Vertical.SpectreLogger.Rendering
{
    /// <summary>
    /// Renders the message portion of the log event.
    /// </summary>
    [Template("{Message}")]
    public class MessageRenderer : ITemplateRenderer
    {
        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventContext context)
        {
            buffer.WriteTemplateValue(context.Profile, destructureValues: false, context.State);
        }
    }
}