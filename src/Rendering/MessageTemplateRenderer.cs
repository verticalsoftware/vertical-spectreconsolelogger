using System.Linq;
using Spectre.Console;
using Vertical.SpectreLogger.Internal;
using Vertical.SpectreLogger.Output;

namespace Vertical.SpectreLogger.Rendering
{
    [Template("{Message}")]
    public class MessageTemplateRenderer : ITemplateRenderer
    {
        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, ref LogEventInfo eventInfo)
        {
            var profile = eventInfo.FormattingProfile;
            var logValues = eventInfo.FormattedLogValues;

            if (!logValues.Any())
            {
                FormattingHelper.RenderLogValue(profile, buffer, eventInfo.State);
                return;
            }

            if (!logValues.TryGetValue("{OriginalFormat}", out var value) || value is not string template)
            {
                // What to do now?
                return;
            }

            // Render each part of the template
            foreach (var match in TemplateParser.Parse(template, preserveFormat: false))
            {
                switch (match)
                {
                    case { isTemplate: true } when logValues.TryGetValue(match.token, out value):
                        FormattingHelper.RenderLogValue(profile, buffer, value);
                        break;
                    
                    default:
                        buffer.Write(match.token.EscapeMarkup());
                        break;
                }
            }
        }
    }
}