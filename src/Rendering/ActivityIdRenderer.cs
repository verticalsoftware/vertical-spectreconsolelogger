using System.Diagnostics;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.Templates;

namespace Vertical.SpectreLogger.Rendering
{
    /// <summary>
    /// Prints the activity id if available.
    /// </summary>
    public class ActivityIdRenderer : ITemplateRenderer
    {
        [Template]
        public static readonly string Template = TemplatePatternBuilder
            .ForKey("[Aa]ctivity[Ii]d")
            .AddAlignment()
            .AddFormatting()
            .Build();
        
        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventContext context)
        {
            var activity = Activity.Current;
            
            if (activity == null)
                return;
            
            buffer.WriteLogValue(
                context.Profile, 
                null,
                activity.TraceId);
        }
    }
}