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
        [Template]
        public static readonly string Template = TemplatePatternBuilder
            .ForKey("ThreadId")
            .AddAlignment()
            .Build();
        
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

        [TypeFormatter(typeof(Thread))]
        public class DefaultFormatter : ICustomFormatter
        {
            /// <inheritdoc />
            public string Format(string? format, object? arg, IFormatProvider? formatProvider) =>
                ((Thread) arg!).ManagedThreadId.ToString();
        }

        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventContext context)
        {
            buffer.WriteLogValue(
                context.Profile,
                null,
                new Value());
        }
    }
}