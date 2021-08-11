using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.PseudoTypes;
using Vertical.SpectreLogger.Templates;

namespace Vertical.SpectreLogger.Rendering
{
    public class KeyedPropertyValueRenderer : ITemplateRenderer
    {
        private readonly TemplateContext _templateContext;
        private readonly string _key;

        [TemplateProvider]
        public static readonly Template Template = new()
        {
            RendererKey = @"Property\.(?<key>\w+)",
            FieldWidthFormatting = true,
            CompositeFormatting = true
        };
        
        public class Options : MultiTypeRenderingOptions
        {
        }

        public KeyedPropertyValueRenderer(TemplateContext templateContext)
        {
            _templateContext = templateContext;
            _key = templateContext.MatchContext.Groups["key"].Value;
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
            var formattedValue = FormattingHelper.FormatValue(options, 
                logValue, 
                type, 
                _templateContext.FieldWidth, 
                _templateContext.CompositeFormat);
            var markup = FormattingHelper.MarkupValue(options, logValue, type);
            
            buffer.Write(formattedValue, markup);
        }
    }
}