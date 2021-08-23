using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Formatting;

namespace Vertical.SpectreLogger.Rendering
{
    [Template(@"{LogLevel" + TemplatePatterns.WidthAndCompositeFormatPattern + "}")]
    public partial class LogLevelRenderer : FormattedValueRenderer
    {
        private readonly TemplateContext _templateContext;

        /// <inheritdoc />
        public LogLevelRenderer(TemplateContext templateContext, RenderedValueCache cache) 
            : base(templateContext, 
                Formatter.Default, 
                FormattingOptions.CompositeFormat, 
                cache)
        {
            _templateContext = templateContext;
        }

        /// <inheritdoc />
        protected override object GetRenderValue(in LogEventInfo eventInfo)
        {
            return eventInfo.LogLevel;
        }
        
        /// <inheritdoc />
        public override string ToString() => $"LogLevel (format=\"{_templateContext.Format}\")";
    }
}