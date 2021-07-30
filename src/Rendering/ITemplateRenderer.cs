using Vertical.SpectreLogger.Output;

namespace Vertical.SpectreLogger.Rendering
{
    /// <summary>
    /// Formats content to the output stream.
    /// </summary>
    public interface ITemplateRenderer
    {
        /// <summary>
        /// Formats the template it is responsible for to an output stream.
        /// </summary>
        /// <param name="buffer">Output buffer to write data to.</param>
        /// <param name="eventInfo">Event info.</param>
        void Render(IWriteBuffer buffer, ref LogEventInfo eventInfo);
    }
}