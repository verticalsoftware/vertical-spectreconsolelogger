using System;
using Vertical.SpectreLogger.Core;

namespace Vertical.SpectreLogger.Rendering
{
    [Template("{DateTime" + TemplatePatterns.WidthAndCompositeFormatPattern +"}")]
    public class DateTimeRenderer : FormattedValueRenderer
    {
        /// <inheritdoc />
        public DateTimeRenderer(TemplateContext templateContext) : base(templateContext)
        {
        }

        /// <inheritdoc />
        protected override object GetRenderValue(in LogEventInfo eventInfo)
        {
            return DateTime.Now;
        }
    }
}