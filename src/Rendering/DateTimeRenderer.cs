using System;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.Templates;

namespace Vertical.SpectreLogger.Rendering
{
    public class DateTimeRenderer : ITemplateRenderer
    {
        private readonly TemplateSegment _template;

        /// <summary>
        /// Options for <see cref="DateTimeRenderer"/>
        /// </summary>
        public sealed class Options
        {
            /// <summary>
            /// Gets or sets a function that returns the desired
            /// <see cref="DateTimeOffset"/>
            /// </summary>
            public Func<DateTimeOffset>? ValueFactory { get; set; } = () => DateTimeOffset.Now;
        }
        
        [Template()]
        public static readonly string Template = TemplatePatternBuilder
            .ForKey("Date[Tt]ime")
            .AddAlignmentGroup()
            .AddFormattingGroup()
            .Build();
            
        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        /// <param name="template">Matching template segment.</param>
        public DateTimeRenderer(TemplateSegment template) => _template = template;

        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventContext context)
        {
            var renderValue = context
                .Profile
                .ConfiguredOptions
                .GetOptions<Options>()
                .ValueFactory?.Invoke() ?? DateTimeOffset.Now;
            
            buffer.WriteFormattedValue(
                context.Profile,
                _template,
                renderValue);
        }
    }
}