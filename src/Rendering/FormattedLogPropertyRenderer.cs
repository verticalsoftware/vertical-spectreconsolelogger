using System.Text.RegularExpressions;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Output;

namespace Vertical.SpectreLogger.Rendering
{
    internal class FormattedLogPropertyRenderer : ITemplateRenderer
    {
        private readonly string _key;
        private readonly string _alignment;
        private readonly string _format;

        internal FormattedLogPropertyRenderer(string templateContext)
        {
            var match = Regex.Match(templateContext, @"{(\w+)(,-?[0-9]+)?(:\w+)?}");

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
            var profileFormattedValue = FormattingHelper.GetProfileFormatOrDefault(profile, logValue);
            var markup = FormattingHelper.GetProfileMarkupOrDefault(profile, logValue);

            if (profileFormattedValue == null)
                return;

            if (_alignment.Length > 0 || _format.Length == 0)
            {
                profileFormattedValue = FormattingHelper.GetCompositeFormat(profileFormattedValue,
                    _alignment,
                    _format);
            }
            
            buffer.Write(profileFormattedValue!, markup);
        }

    }
}