using System.Text.RegularExpressions;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.PseudoTypes;
using Vertical.SpectreLogger.Templates;

namespace Vertical.SpectreLogger.Rendering
{
    [Template(@"{Property:(\w+)(,-?\d+)?(:[^}]+)?}")]
    public class KeyedPropertyValueRenderer : ITemplateRenderer
    {
        private readonly string _key;
        private readonly string _width;
        private readonly string _format;

        public class Options : MultiTypeRenderingOptions
        {
        }

        public KeyedPropertyValueRenderer(Match matchContext)
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

            logValue ??= NullValue.Default;

            var type = logValue.GetType();
            var options = eventInfo.FormattingProfile.GetRendererOptions<Options>();
            var formattedValue = FormattingHelper.FormatValue(options, logValue, type, _width, _format);
            var markup = FormattingHelper.MarkupValue(options, logValue, type);
            
            buffer.Write(formattedValue, markup);
        }
    }
}