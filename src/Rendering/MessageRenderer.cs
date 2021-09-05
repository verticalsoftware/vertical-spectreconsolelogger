using System.Collections.Generic;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Formatting;
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
            var profile = context.Profile;

            if (state is not IReadOnlyList<KeyValuePair<string, object>> formattedLogValues)
            {
                buffer.WriteLogValue(profile, null, state ?? NullValue.Default);
                return;
            }

            if (!formattedLogValues.TryGetValue("{OriginalFormat}", out var originalFormat))
            {
                buffer.WriteLogValue(profile, null, state);
                return;
            }

            if (originalFormat is not string originalFormatString)
            {
                buffer.WriteLogValue(profile, null, state);
                return;
            }

            TemplateString.Split(originalFormatString, (in TemplateSegment segment) =>
            {
                if (segment.IsTemplate && formattedLogValues.TryGetValue(segment.Key!, out var logValue))
                {
                    buffer.WriteLogValue(profile, segment, logValue ?? NullValue.Default);
                    return;
                }
                
                buffer.Write(segment.Value);
            });
        }
    }
}