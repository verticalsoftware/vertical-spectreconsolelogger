using Vertical.SpectreLogger.Output;

namespace Vertical.SpectreLogger.Rendering
{
    internal class EndEventRenderer : ITemplateRenderer
    {
        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventInfo eventInfo)
        {
            buffer.Margin = 0;
            buffer.WriteLine();
        }
    }
}