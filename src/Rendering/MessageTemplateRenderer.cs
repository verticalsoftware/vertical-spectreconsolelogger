using System.Collections.Generic;
using System.Linq;
using Vertical.SpectreLogger.Internal;
using Vertical.SpectreLogger.MatchableTypes;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Output;

namespace Vertical.SpectreLogger.Rendering
{
    [Template("{Message}")]
    public class MessageTemplateRenderer : ITemplateRenderer
    {
        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, ref LogEventInfo eventInfo)
        {
            var profile = eventInfo.FormattingProfile;

            if (eventInfo.State is not IEnumerable<KeyValuePair<string, object>> propertyCollection)
            {
                WriteValue(buffer, eventInfo.State, profile);
                return;
            }

            var properties = propertyCollection.ToDictionary(kv => kv.Key, kv => kv.Value);

            if (!properties.TryGetValue("{OriginalFormat}", out var value) || value is not string template)
            {
                // What to do now?
                return;
            }

            // Render each part of the template
            foreach (var match in TemplateParser.Parse(template, preserveFormat: false))
            {
                switch (match)
                {
                    case { isTemplate: true } when properties.TryGetValue(match.token, out value):
                        WriteValue(buffer, value, profile);
                        break;
                    
                    default:
                        buffer.Append(profile, match.token);
                        break;
                }
            }
        }

        private static void WriteValue(IWriteBuffer buffer, object? obj, FormattingProfile profile)
        {
            // Resolve the value to display
            var type = obj?.GetType() ?? typeof(Null);
            
            var formatter = profile.ValueFormatters.GetValueOrDefault(type)
                            ??
                            profile.ValueFormatters.GetValueOrDefault(typeof(object));
            
            var formattedValue = formatter?.Invoke(obj) ?? obj?.ToString();

            // Nothing to render
            if (string.IsNullOrEmpty(formattedValue))
                return;

            var markup = profile.TypeStyles.GetValueOrDefault(type)
                         ??
                         profile.TypeStyles.GetValueOrDefault(typeof(object));
            
            buffer.Append(profile, formattedValue!, markup);
        }
    }
}