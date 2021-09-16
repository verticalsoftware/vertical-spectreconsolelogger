using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Output;

namespace Vertical.SpectreLogger.Rendering
{
    internal sealed class EndEventRenderer : ITemplateRenderer
    {
        internal static readonly ITemplateRenderer Default = new EndEventRenderer();
        
        private EndEventRenderer()
        {
        }
        
        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventContext context)
        {
            buffer.Margin = 0;

            if (buffer.LinePosition != 0)
            {
                buffer.WriteLine();
            }
        }
    }
}