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
        /// <param name="logEventContext">Log event info</param>
        void Render(IWriteBuffer buffer, in LogEventContext logEventContext);
    }
}