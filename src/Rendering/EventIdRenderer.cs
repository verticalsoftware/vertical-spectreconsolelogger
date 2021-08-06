using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Output;

namespace Vertical.SpectreLogger.Rendering
{
    [Template(MyTemplate)]
    public class EventIdRenderer : ITemplateRenderer
    {
        private const string MyTemplate = @"{EventId(,-?\d+)?(?::(Id|Name))?}";

        public class Options : ValueRenderingOptions<EventId>
        {
        }

        private readonly string _alignment;
        private readonly string _format;

        public EventIdRenderer(string templateContext)
        {
            var match = Regex.Match(templateContext, MyTemplate);

            _alignment = match.Groups[1].Value;
            _format = match.Groups[2].Value;
        }

        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventInfo eventInfo)
        {
            var options = eventInfo.FormattingProfile.GetRenderingOptions<Options>();
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
                ? FormattingHelper.GetCompositeFormat(valueFormat, _alignment)
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