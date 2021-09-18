using System.Text.RegularExpressions;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.Templates;

namespace Vertical.SpectreLogger.Rendering
{
    [Template(@"{New[Ll]ine(?<_q>\+)?}")]
    public class NewLineRenderer : ITemplateRenderer
    {
        private readonly bool _queueNewLine; 

        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        public NewLineRenderer(Match match)
        {
            _queueNewLine = match.Groups["_q"].Success;
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