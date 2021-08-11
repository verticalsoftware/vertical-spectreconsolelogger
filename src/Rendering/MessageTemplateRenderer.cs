using System.Linq;
using System.Text.RegularExpressions;
using Spectre.Console;
using Vertical.SpectreLogger.Internal;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.PseudoTypes;
using Vertical.SpectreLogger.Templates;
using Vertical.SpectreLogger.Utilities;

namespace Vertical.SpectreLogger.Rendering
{
    public class MessageTemplateRenderer : ITemplateRenderer
    {
        private readonly TemplateContext _templateContext;

        public class Options : MultiTypeRenderingOptions
        {
        }

        [TemplateProvider]
        public static readonly Template Template = new()
        {
            RendererKey = "Message",
            NewLineControl = true,
            MarginControl = true
        };
        
        public MessageTemplateRenderer(TemplateContext templateContext)
        {
            _templateContext = templateContext;
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

            buffer.WriteLine(_templateContext);

            // Render each part of the template
            template.SplitTemplate(match =>
            {
                var (token, isTemplate) = match;
                
                if (isTemplate && logValues.TryGetValue(token.Substring(1, token.Length-2), out var logValue))
                {
                    logValue ??= NullValue.Default;

                    var type = logValue.GetType();
                    var formattedValue = FormattingHelper.FormatValue(options, logValue, type, 
                        _templateContext.FieldWidth, 
                        _templateContext.CompositeFormat);
                    var markup = FormattingHelper.MarkupValue(options, logValue, type);
                    
                    buffer.Write(formattedValue, markup);
                    
                    return;
                }
                
                buffer.Write(token.EscapeMarkup());
            });
        }
    }
}