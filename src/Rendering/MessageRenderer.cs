using System.Collections.Generic;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Internal;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.Templates;

namespace Vertical.SpectreLogger.Rendering
{
    [Template("{Message}")]
    public class MessageRenderer : ITemplateRenderer
    {
        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventInfo eventInfo)
        {
            var state = eventInfo.State;

            if (state is not IReadOnlyList<KeyValuePair<string, object>> formattedLogValues)
            {
                // TODO: Render as what?
                return;
            }

            if (!formattedLogValues.TryGetValue("{OriginalFormat}", out var originalFormat))
            {
                // TODO: Not sure what to do here...
            }

            if (originalFormat is not string originalFormatString)
            {
                // TODO: Render as ToString?
                return;
            }
            
            TemplateString.Split(originalFormatString, (in TemplateSegment segment) =>
            {
                if (segment.IsTemplate && formattedLogValues.TryGetValue(segment.InnerTemplate!, out var value))
                {
                    
                }
            });
        }
    }
}