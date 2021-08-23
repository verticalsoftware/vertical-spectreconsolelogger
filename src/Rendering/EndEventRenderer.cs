using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.Utilities;

namespace Vertical.SpectreLogger.Rendering
{
    internal class EndEventRenderer : ITemplateRenderer
    {
        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventInfo eventInfo)
        {
            var lfRequired = !buffer.AtMargin();
            
            buffer.ClearEnqueued();
            buffer.Margin = 0;

            if (lfRequired)
            {
                buffer.WriteLine();
            }
        }
    }
}