using System;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Formatting;
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
        
        [Template]
        public static readonly string Template = TemplatePatternBuilder
            .ForKey("[Dd]ate[Tt]ime")
            .AddAlignment()
            .AddFormatting()
            .Build();

        /// <summary>
        /// Emits the date/time value
        /// </summary>
        public class Value : ValueWrapper<DateTimeOffset>
        {
            /// <inheritdoc />
            public Value(DateTimeOffset value) : base(value)
            {
            }
        }

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
            
            buffer.WriteLogValue(
                context.Profile,
                _template,
                new Value(renderValue));
        }
    }
}