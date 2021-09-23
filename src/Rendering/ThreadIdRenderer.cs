using System;
using System.Threading;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Formatting;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.Templates;

namespace Vertical.SpectreLogger.Rendering
{
    /// <summary>
    /// Renders the thread id (at the time of capture).
    /// </summary>
    public class ThreadIdRenderer : ITemplateRenderer
    {
        /// <summary>
        /// Defines the template for this renderer.
        /// </summary>
        [Template]
        public static readonly string Template = TemplatePatternBuilder
            .ForKey("ThreadId")
            .AddAlignment()
            .AddFormatting()
            .Build();

        private readonly TemplateSegment _template;

        /// <summary>
        /// Wraps the thread value.
        /// </summary>
        public class Value : ValueWrapper<Thread>
        {
            /// <summary>
            /// Creates a new instance of this type. The thread id is automatically assigned.
            /// </summary>
            public Value() : base(Thread.CurrentThread)
            {
            }
        }

        /// <summary>
        /// The default formatter for this type.
        /// </summary>
        [TypeFormatter(typeof(Value))]
        public class DefaultFormatter : ICustomFormatter
        {
            /// <inheritdoc />
            public string Format(string? format, object? arg, IFormatProvider? formatProvider) =>
                ((Value) arg!).Value.ManagedThreadId.ToString(format, formatProvider);
        }

        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        /// <param name="template">Template segment</param>
        public ThreadIdRenderer(TemplateSegment template) => _template = template;

        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventContext context)
        {
            buffer.WriteLogValue(
                context.Profile,
                _template,
                new Value());
        }
    }
}