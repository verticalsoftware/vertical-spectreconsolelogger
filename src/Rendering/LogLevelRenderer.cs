using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Internal;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.Templates;

namespace Vertical.SpectreLogger.Rendering
{
    /// <summary>
    /// Renders the log level.
    /// </summary>
    public class LogLevelRenderer : ITemplateRenderer
    {
        private readonly TemplateSegment _template;

        [Template] 
        public static readonly string Template = TemplatePatternBuilder
            .ForKey("LogLevel")
            .AddAlignmentGroup()
            .AddFormattingGroup()
            .Build();

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="template">The matching template segment.</param>
        public LogLevelRenderer(TemplateSegment template) => _template = template;

        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventContext context)
        {
            buffer.WriteLogValue(context.Profile, _template, context.LogLevel);
        }
    }
}