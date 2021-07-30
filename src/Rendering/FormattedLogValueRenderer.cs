using System.Text.RegularExpressions;
using Vertical.SpectreLogger.MatchableTypes;
using Vertical.SpectreLogger.Output;

namespace Vertical.SpectreLogger.Rendering
{
    internal class FormattedLogValueRenderer : ITemplateRenderer
    {
        private readonly string _key;
        private readonly string _format;

        internal FormattedLogValueRenderer(string templateContext)
        {
            var match = Regex.Match(templateContext, @"{(\w+)([,:][^}]+)?}");

            _key = match.Groups[1].Value;
            _format = match.Groups[2].Value;
        }
        
        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, ref LogEventInfo eventInfo)
        {
            var logValues = eventInfo.FormattedLogValues;

            if (!logValues.TryGetValue(_key, out var logValue))
                return;
        }
    }
}