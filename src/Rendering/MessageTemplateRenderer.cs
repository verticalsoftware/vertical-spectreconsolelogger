using System.Linq;
using System.Text.RegularExpressions;
using Spectre.Console;
using Vertical.SpectreLogger.Internal;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.PseudoTypes;
using Vertical.SpectreLogger.Templates;

namespace Vertical.SpectreLogger.Rendering
{
    [Template(@"{Message(:NewLine(\?)?(?::(-?\d+)(!)?)?)?}")]
    public class MessageTemplateRenderer : ITemplateRenderer
    {
        public class Options : MultiTypeRenderingOptions
        {
        }

        private readonly bool _newLine;
        private readonly bool _newLineConditional;
        private readonly int? _margin;
        private readonly bool _assign;
        
        public MessageTemplateRenderer(Match matchContext)
        {
            _newLine = matchContext.Groups[1].Success;
            _newLineConditional = matchContext.Groups[2].Success;
            _margin = matchContext.Groups[3].Success ? int.Parse(matchContext.Groups[3].Value) : null;
            _assign = matchContext.Groups[4].Success;
        }
        
        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventInfo eventInfo)
        {
            var profile = eventInfo.FormattingProfile;
            var options = profile.GetRendererOptions<Options>();
            var logValues = eventInfo.FormattedLogValues;

            if (!logValues.TryGetValue("{OriginalFormat}", out var value) || value is not string template)
            {
                // What to do now?
                return;
            }

            if (_newLine && (!buffer.AtMargin || !_newLineConditional))
            {
                buffer.WriteLine();
                if (_margin.HasValue)
                {
                    buffer.Margin = _assign
                        ? _margin.Value
                        : buffer.Margin + _margin.Value;
                }
            }

            // Render each part of the template
            ParseUtilities.EnumerateTokens(template, (match, token) =>
            {
                if (match != null && logValues.TryGetValue(token, out var logValue))
                {
                    logValue ??= NullValue.Default;

                    var type = logValue.GetType();
                    var width = match.Groups[2].Value;
                    var format = match.Groups[3].Value;
                    var formattedValue = FormattingHelper.FormatValue(options, logValue, type, width, format);
                    
                    var markup = FormattingHelper.MarkupValue(options, logValue, type);
                    buffer.Write(formattedValue, markup);

                    return;
                }
                
                buffer.Write(token.EscapeMarkup());
            });
        }
    }
}