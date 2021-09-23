using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.Templates;

namespace Vertical.SpectreLogger.Rendering
{
    /// <summary>
    /// Renders a new line.
    /// </summary>
    [Template("{NewLine}")]
    public class NewLineRenderer : ITemplateRenderer
    {
        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventContext context) => buffer.WriteLine();
    }
}