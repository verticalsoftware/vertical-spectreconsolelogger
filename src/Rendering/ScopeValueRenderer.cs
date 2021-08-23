using System.Collections.Generic;
using System.Linq;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.Types;
using Vertical.SpectreLogger.Utilities;

namespace Vertical.SpectreLogger.Rendering
{
    /// <summary>
    /// Renders a single scope value.
    /// </summary>
    [Template(@"{Scope=(?<key>[^:,}]+)" + TemplatePatterns.WidthAndCompositeFormatPattern + "}")]
    public class ScopeValueRenderer : ITemplateRenderer
    {
        private readonly TemplateContext _templateContext;
        private readonly string _key;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="templateContext">Template context.</param>
        public ScopeValueRenderer(TemplateContext templateContext)
        {
            _templateContext = templateContext;
            _key = templateContext.Match.Groups["key"].Value;
        }

        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventInfo eventInfo)
        {
            var scopeValue = GetScopeValue(eventInfo.Scopes, _key);

            buffer.WriteFormattedValue(
                scopeValue ?? NullLogValue.Default,
                eventInfo.FormattingProfile,
                _templateContext);
        }

        private static object? GetScopeValue(object?[] scopes, string key)
        {
            var length = scopes.Length;
            
            for(var c = 0; c < length; c++)
            {
                if (scopes[c] is not IReadOnlyList<KeyValuePair<string, object>> logValues)
                    continue;

                var keyValuePair = logValues.FirstOrDefault(kv => kv.Key == key);

                if (ReferenceEquals(keyValuePair.Key, null))
                    continue;

                return keyValuePair.Value;
            }

            return null;
        }

        /// <inheritdoc />
        public override string ToString() => $"Scope=\"{_key}\"";
    }
}