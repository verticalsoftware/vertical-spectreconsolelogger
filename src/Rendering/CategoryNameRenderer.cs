using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.Templates;

namespace Vertical.SpectreLogger.Rendering
{
    public class CategoryNameRenderer : ITemplateRenderer
    {
        private readonly TemplateSegment _template;

        [Template]
        public static readonly string Template = TemplatePatternBuilder
            .ForKey("CategoryName")
            .AddWidthFormatting()
            .AddValueFormatting()
            .Build();

        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        /// <param name="template">Matching segment from the output template.</param>
        public CategoryNameRenderer(TemplateSegment template) => _template = template;
        
        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventContext context)
        {
            buffer.WriteLogValueFormat(
                context.Profile,
                _template,
                context.CategoryName);
        }
    }
}