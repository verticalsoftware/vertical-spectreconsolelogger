using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Formatting;

namespace Vertical.SpectreLogger.Rendering
{
    [Template((@"{CategoryName" + TemplatePatterns.CompositeFormatPattern + "}"))]
    public partial class CategoryNameRenderer : FormattedValueRenderer
    {
        private readonly TemplateContext _templateContext;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="templateContext">Template context</param>
        /// <param name="cache">Rendered value cache</param>
        public CategoryNameRenderer(TemplateContext templateContext, RenderedValueCache cache) 
            : base(templateContext, 
                Formatter.Instance, 
                FormattingOptions.TemplateFormat, 
                cache)
        {
            _templateContext = templateContext;
        }

        /// <inheritdoc />
        protected override object GetRenderValue(in LogEventInfo eventInfo)
        {
            return eventInfo.CategoryName;
        }
        
        /// <inheritdoc />
        public override string ToString() => $"CategoryName (format=\"{_templateContext.Format}\"";
    }
}