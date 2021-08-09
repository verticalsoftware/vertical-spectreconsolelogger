using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.PseudoTypes;

namespace Vertical.SpectreLogger.Rendering
{
    [Template(@"{Scopes(:NewLine(\?)?(?::(-?\d+)(!)?)?)?}")]
    public class ScopeValuesRenderer : ITemplateRenderer
    {
        private const string DefaultSeparator = " => ";
        
        public class Options : MultiTypeRenderingOptions
        {
            public string? Separator { get; set; } = DefaultSeparator;

            public Func<IEnumerable<object?>, IEnumerable<object?>>? ProviderFilter { get; set; }
        }
        
        private readonly bool _newLine;
        private readonly bool _newLineConditional;
        private readonly int? _margin;
        private readonly bool _assign;

        public ScopeValuesRenderer(Match matchContext)
        {
            _newLine = matchContext.Groups[1].Success;
            _newLineConditional = matchContext.Groups[2].Success;
            _margin = matchContext.Groups[3].Success ? int.Parse(matchContext.Groups[3].Value) : null;
            _assign = matchContext.Groups[4].Success;
        }
        
        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventInfo eventInfo)
        {
            var scopes = eventInfo.Scopes;

            if (scopes.Length == 0)
                return;

            if (_newLine && (!_newLineConditional || !buffer.AtMargin))
            {
                buffer.WriteLine();
                if (_margin.HasValue)
                {
                    buffer.Margin = _assign
                        ? _margin.Value
                        : buffer.Margin + _margin.Value;
                }
            }

            var profile = eventInfo.FormattingProfile;
            var options = profile.GetRendererOptions<Options>();
            var separator = string.Empty;
            var providedScopes = options?.ProviderFilter?.Invoke(scopes) ?? scopes;
            
            foreach (var scope in providedScopes)
            {
                buffer.Write(separator);

                var value = scope ?? NullValue.Default;
                var type = value.GetType();
                var formattedValue = FormattingHelper.FormatValue(options,
                    value,
                    type);
                var markup = FormattingHelper.MarkupValue(options, value, type);
                buffer.Write(formattedValue, markup);

                separator = options?.Separator ?? DefaultSeparator;
            }
        }
    }
}