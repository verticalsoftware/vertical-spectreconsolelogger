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
        public void Render(IWriteBuffer buffer, in LogEventContext context)
        {
            var state = context.State;

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
                switch (segment.IsTemplate)
                {
                    case true:
                        if (formattedLogValues.TryGetValue(segment.Key!, out var logValue))
                        {
                                
                        }
                        
                        break;
                }
            });
        }
    }
}