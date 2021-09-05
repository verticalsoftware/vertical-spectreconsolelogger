using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.Templates;

namespace Vertical.SpectreLogger.Rendering
{
    [Template("{Newline(?<_q>\\+)}")]
    public class NewLineRenderer : ITemplateRenderer
    {
        private readonly bool _queueNewLine; 

        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        /// <param name="template"></param>
        public NewLineRenderer(TemplateSegment template)
        {
            _queueNewLine = template.Match!.Groups["_q"].Success;
        }
        
        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventContext context)
        {
            if (_queueNewLine)
            {
                buffer.EnqueueLine();
            }
            else
            {
                buffer.WriteLine();
            }
        }
    }
}