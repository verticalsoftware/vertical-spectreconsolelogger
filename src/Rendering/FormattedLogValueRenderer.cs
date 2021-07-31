using System.Text.RegularExpressions;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Output;

namespace Vertical.SpectreLogger.Rendering
{
    internal class FormattedLogValueRenderer : ITemplateRenderer
    {
        private readonly string _key;
        private readonly string _alignment;
        private readonly string _format;

        internal FormattedLogValueRenderer(string templateContext)
        {
            var match = Regex.Match(templateContext, @"{(\w+)(,-?\d+)?(:[a-zA-Z0-9]+)?}");

            _key = match.Groups[1].Value;
            _alignment = match.Groups[2].Value;
            _format = match.Groups[3].Value;
        }
        
        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventInfo eventInfo)
        {
            var logValues = eventInfo.FormattedLogValues;

            if (!logValues.TryGetValue(_key, out var logValue))
                return;

            var profile = eventInfo.FormattingProfile;

            var formattedValue = TryUseTemplateFormat(profile, logValue) 
                                 ?? 
                                 FormattingHelper.CreateFormattedValue(profile, logValue);

            if (!formattedValue.HasValue)
                return;
            
            buffer.Write(formattedValue);
        }

        private FormattedValue? TryUseTemplateFormat(FormattingProfile profile, object? logValue)
        {
            if (_format.Length == 0)
                return null;

            var formatString = $"{{0{_alignment}{_format}}}";

            return new FormattedValue(formatString, FormattingHelper.FormatMarkup(profile, logValue));
        }
    }
}