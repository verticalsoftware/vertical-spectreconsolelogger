using Microsoft.Extensions.Logging;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.Templates;

namespace Vertical.SpectreLogger.Rendering
{
    public class EventIdRenderer : ITemplateRenderer
    {
        private readonly TemplateContext _templateContext;
        
        [TemplateProvider]
        public static readonly Template Descriptor = new ()
        {
            RendererKey = "EventId",
            FieldWidthFormatting = true,
            CustomFormat = "(:(?<property>Id|Name))?"
        };

        public class Options : TypeRenderingOptions<EventId>
        {
        }

        private readonly string _format;

        public EventIdRenderer(TemplateContext templateContext)
        {
            _templateContext = templateContext;
            _format = templateContext.MatchContext.Groups["property"].Value;
        }
        
        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventInfo eventInfo)
        {
            var options = eventInfo.FormattingProfile.GetRendererOptions<Options>();
            var formattedValue = Format(eventInfo.EventId, options);

            if (formattedValue == null)
                return;

            buffer.Write(formattedValue, options?.Style);
        }

        private string? Format(EventId eventId, Options? options)
        {
            var valueFormat = options switch
            {
                { Formatter: {}} => options.Formatter(eventId),
                { } when _format.Length > 0 => FormatToProperty(eventId, _format),
                _ => eventId.ToString()
            };

            return valueFormat != null
                ? FormattingHelper.GetCompositeFormat(valueFormat, _templateContext.FieldWidth)
                : null;
        }

        private static string FormatToProperty(EventId eventId, string format)
        {
            return format switch
            {
                "Id" => eventId.Id.ToString(),
                _ => eventId.Name
            };
        }
    }
}