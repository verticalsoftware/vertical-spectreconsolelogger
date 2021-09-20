using System.Text.RegularExpressions;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Formatting;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.Templates;

namespace Vertical.SpectreLogger.Rendering
{
    [Template(@"{New[Ll]ine(?<_q>\+)?}")]
    public class NewLineRenderer : ITemplateRenderer
    {
        public static readonly string Template = TemplatePatternBuilder
            .ForKey("[Nn]ew[Ll]ine")
            .Build();
        
        private readonly bool _queueNewLine;

        public NewLineRenderer(TemplateSegment template) => _queueNewLine = template.ControlCodeMatched;

        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        public NewLineRenderer(Match match)
        {
            _queueNewLine = match.Groups["_q"].Success;
        }

        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventContext context) => buffer.EnqueueLine();
    }
}