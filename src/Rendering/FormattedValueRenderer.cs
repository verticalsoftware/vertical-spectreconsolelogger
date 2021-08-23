using System;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Formatting;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.Utilities;

namespace Vertical.SpectreLogger.Rendering
{
    public abstract class FormattedValueRenderer : ITemplateRenderer
    {
        private readonly IFormatter? _formatter;
        private readonly FormattingOptions _formattingOptions;
        private readonly TemplateContext _templateContext;
        private readonly RenderedValueCache? _valueCache;

        private readonly Type _type;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="templateContext">Template context</param>
        /// <param name="formatter">Formatter</param>
        /// <param name="formattingOptions">Formatting options</param>
        /// <param name="valueCache">Optional value cache</param>
        protected FormattedValueRenderer(TemplateContext templateContext, 
            IFormatter? formatter = null,
            FormattingOptions formattingOptions = FormattingOptions.All,
            RenderedValueCache? valueCache = null)
        {
            _formatter = formatter;
            _formattingOptions = formattingOptions;
            _templateContext = templateContext;
            _valueCache = valueCache;
            _type = GetType();
        }

        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventInfo eventInfo)
        {
            if (_valueCache != null)
            {
                RenderCached(buffer, eventInfo);
                return;
            }    
            
            buffer.WriteFormattedValue(
                GetRenderValue(eventInfo),
                eventInfo.FormattingProfile,
                _templateContext,
                _formatter,
                _formattingOptions);
        }

        private void RenderCached(IWriteBuffer buffer, in LogEventInfo eventInfo)
        {
            var originalValue = GetRenderValue(eventInfo);

            if (_valueCache!.TryGetValue(_type, originalValue, out var cachedValue))
            {
                buffer.Write(cachedValue!);
                return;
            }

            buffer.WriteFormattedValue(
                out var renderedValue,
                originalValue,
                eventInfo.FormattingProfile,
                _templateContext,
                _formatter,
                _formattingOptions);
            
            _valueCache.CacheValue(_type, originalValue, renderedValue);
        }

        /// <summary>
        /// Computes the value to render.
        /// </summary>
        /// <returns></returns>
        protected abstract object GetRenderValue(in LogEventInfo eventInfo);
    }
}