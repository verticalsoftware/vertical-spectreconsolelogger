using Vertical.SpectreLogger.Output;

namespace Vertical.SpectreLogger.Core
{
    /// <summary>
    /// Represents an object that renders log event data to the write buffer.
    /// </summary>
    public interface ITemplateRenderer
    {
        /// <summary>
        /// Renders the event data specific to the template.
        /// </summary>
        /// <param name="buffer">The buffer to write the data to.</param>
        /// <param name="eventInfo"><see cref="LogEventInfo"/> that describes the event data.</param>
        void Render(IWriteBuffer buffer, in LogEventInfo eventInfo);
    }
}