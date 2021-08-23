using System;
using Vertical.SpectreLogger.Core;

namespace Vertical.SpectreLogger.Rendering
{
    [Template("{DateTime" + TemplatePatterns.WidthAndCompositeFormatPattern +"}")]
    public class DateTimeOffsetRenderer : FormattedValueRenderer
    {
        /// <inheritdoc />
        public DateTimeOffsetRenderer(TemplateContext templateContext) : base(templateContext)
        {
        }

        /// <inheritdoc />
        protected override object GetRenderValue(in LogEventInfo eventInfo)
        {
            return DateTimeOffset.Now;
        }
    }
}