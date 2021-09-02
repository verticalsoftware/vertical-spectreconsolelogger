using Vertical.SpectreLogger.Output;

namespace Vertical.SpectreLogger.Core
{
    /// <summary>
    /// An object that renders the content represented by a template.
    /// </summary>
    public interface ITemplateRenderer
    {
        /// <summary>
        /// Renders the template portion of the log event to the provided buffer.
        /// </summary>
        /// <param name="buffer">Write buffer</param>
        /// <param name="context">Log event data.</param>
        void Render(IWriteBuffer buffer, in LogEventContext context);
    }
}