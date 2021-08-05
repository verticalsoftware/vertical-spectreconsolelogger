using System.Text.RegularExpressions;
using Vertical.SpectreLogger.Output;

namespace Vertical.SpectreLogger.Rendering
{
    [Template(MyTemplate)]
    public class EventIdRenderer : ITemplateRenderer
    {
        private const string MyTemplate = @"{EventId(,\d+)?(:\w+)?}";

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
            
        }
    }
}