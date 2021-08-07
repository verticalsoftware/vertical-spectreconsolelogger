using System.Linq;
using Spectre.Console;
using Vertical.SpectreLogger.Internal;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.PseudoTypes;

namespace Vertical.SpectreLogger.Rendering
{
    [Template("{Message}")]
    public class MessageTemplateRenderer : ITemplateRenderer
    {
        public class Options : MultiTypeRenderingOptions
        {
        }
        
        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventInfo eventInfo)
        {
            var profile = eventInfo.FormattingProfile;
            var options = profile.GetRendererOptions<Options>();
            var logValues = eventInfo.FormattedLogValues;

            if (!logValues.Any())
            {
                FormattingHelper.RenderFormattedValue(profile, buffer, eventInfo.State);
                return;
            }

            if (!logValues.TryGetValue("{OriginalFormat}", out var value) || value is not string template)
            {
                // What to do now?
                return;
            }

            // Render each part of the template
            TemplateParser.GetTokens(template, (match, token) =>
            {
                if (match != null)
                {
                    if (!logValues.TryGetValue(token, out var logValue))
                        return;

                    var type = logValue?.GetType() ?? typeof(NullValue);
                    var width = match.Groups[2].Value;
                    var format = match.Groups[3].Value;
                    var formattedValue = FormattingHelper.FormatValue(options, logValue, type, width, format);

                    if (formattedValue == null)
                        return;

                    var markup = FormattingHelper.MarkupValue(options, logValue, type);
                    
                    buffer.Write(formattedValue, markup);
                    
                    return;
                }
                
                buffer.Write(token.EscapeMarkup());
            });
        }
    }
}