using System.Text.RegularExpressions;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.PseudoTypes;

namespace Vertical.SpectreLogger.Rendering
{
    public class FormattedLogValueRenderer : ITemplateRenderer
    {
        private readonly string _key;
        private readonly string _width;
        private readonly string _format;

        public class Options : MultiTypeRenderingOptions
        {
        }

        internal FormattedLogValueRenderer(string token)
        {
            // Rematch
            var match = Regex.Match(token, @"{(\w+)(,-?[0-9]+)?(:\w+)?}");

            _key = match.Groups[1].Value;
            _width = match.Groups[2].Value;
            _format = match.Groups[3].Value;
        }
        
        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventInfo eventInfo)
        {
            var logValues = eventInfo.FormattedLogValues;

            if (!logValues.TryGetValue(_key, out var logValue))
                return;

            var type = logValue?.GetType() ?? typeof(NullValue);
            var options = eventInfo.FormattingProfile.GetRendererOptions<Options>();
            var formattedValue = FormattingHelper.FormatValue(options, logValue, type, _width, _format);

            if (formattedValue == null)
                return;

            var markup = FormattingHelper.MarkupValue(options, logValue, type);
            buffer.Write(formattedValue, markup);
        }

    }
}