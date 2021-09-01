using Vertical.SpectreLogger.Output;

namespace Vertical.SpectreLogger.Core
{
    /// <summary>
    /// Defines a renderer pipeline.
    /// </summary>
    public interface IRendererPipeline
    {
        /// <summary>
        /// Renders the log event.
        /// </summary>
        /// <param name="buffer">The buffer to stage the output to.</param>
        /// <param name="logEventInfo">Log event info</param>
        void Render(IWriteBuffer buffer, in LogEventInfo logEventInfo);
    }
}