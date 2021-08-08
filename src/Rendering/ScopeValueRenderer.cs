using System.Text.RegularExpressions;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.PseudoTypes;

namespace Vertical.SpectreLogger.Rendering
{
    [Template(@"{Scope:(\w+)(,-?\d+)?(:[^}]+)?}")]
    public class ScopeValueRenderer : ITemplateRenderer
    {
        private readonly string _key;
        private readonly string _width;
        private readonly string _format;

        public class Options : MultiTypeRenderingOptions
        {
        }

        public ScopeValueRenderer(Match matchContext)
        {
            _key = matchContext.Groups[1].Value;
            _width = matchContext.Groups[2].Value;
            _format = matchContext.Groups[3].Value;
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