using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.Utilities;

namespace Vertical.SpectreLogger.Rendering
{
    [Template((@"{CategoryName" + TemplatePatterns.CompositeFormatPattern + "}"))]
    public class CategoryNameRenderer : ITemplateRenderer
    {
        private readonly TemplateContext _templateContext;
        private readonly ConcurrentDictionary<(LogLevel, string), string> _cache = new();

        public CategoryNameRenderer(TemplateContext templateContext)
        {
            _templateContext = templateContext;
        }
        
        private class Formatter : IFormatter
        {
            internal static readonly IFormatter Instance = new Formatter();
            
            /// <inheritdoc />
            public string Format(string format, object value)
            {
                var category = (string) value;

                switch (format)
                {
                    case "C":
                        // Class only
                        var lastDotIndex = category.LastIndexOf(category, '.');
                        return lastDotIndex > -1 ? category.Substring(lastDotIndex + 1) : category;
                    
                    case "P":
                        // Direct parent + class
                        var count = 0;
                        
                        for (var c = category.Length - 1; c >= 0; c--)
                        {
                            if (c == '.')
                            {
                                count++;
                            }

                            if (count == 2)
                            {
                                return category.Substring(c + 1);
                            }
                        }

                        return category;
                    
                    case "F":
                        // Full name
                        return category;
                    
                    default:
                        throw new InvalidOperationException($"Invalid format for CategoryName: '{format}'");
                }
            }
        }

        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventInfo eventInfo)
        {
            if (string.IsNullOrWhiteSpace(eventInfo.CategoryName))
                return;

            var formattingProfile = eventInfo.FormattingProfile;
            var formattedValue = _cache.GetOrAdd((eventInfo.LogLevel, eventInfo.CategoryName), key => BuildCachedValue(key.Item2, formattingProfile));

            buffer.Write(formattedValue);
        }

        private string BuildCachedValue(string categoryName, FormattingProfile profile)
        {
            using var buffer = new CapturingWriteBuffer();

            buffer.Write(profile, _templateContext, Formatter.Instance, categoryName);

            return buffer.ToString();
        }
    }
}