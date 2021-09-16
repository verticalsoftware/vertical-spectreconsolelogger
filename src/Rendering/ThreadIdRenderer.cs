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
            .AddAlignmentGroup()
            .Build();
        
        /// <summary>
        /// Wraps the thread value.
        /// </summary>
        public class Value : ValueWrapper<int>
        {
            /// <summary>
            /// Creates a new instance of this type. The thread id is automatically assigned.
            /// </summary>
            public Value() : base(Thread.CurrentThread.ManagedThreadId)
            {
            }
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