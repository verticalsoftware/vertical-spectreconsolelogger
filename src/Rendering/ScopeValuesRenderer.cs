using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.Templates;

namespace Vertical.SpectreLogger.Rendering
{
    [Template("{Scopes}")]
    public class ScopeValuesRenderer : ITemplateRenderer
    {
        /// <summary>
        /// Options for <see cref="ScopeValuesRenderer"/>
        /// </summary>
        public sealed class Options
        {
            /// <summary>
            /// Gets or sets content to output before rendering scopes.
            /// </summary>
            public string? ContentBefore { get; set; }

            /// <summary>
            /// Gets or sets content to output between each item.
            /// </summary>
            public string? ContentBetween { get; set; } = " => ";

            /// <summary>
            /// Gets or sets content to output after rendering scopes.
            /// </summary>
            public string? ContentAfter { get; set; } = " => ";
        }
        
        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventContext context)
        {
            var scopeValues = context.ScopeValues;

            if (!scopeValues.HasValues)
                return;

            var profile = context.Profile;
            var options = profile.RendererOptions.GetOptions<Options>();

            if (options.ContentBefore != null)
            {
                buffer.Write(options.ContentBefore);
            }

            var items = scopeValues.Values;
            var length = items.Count;

            for (var c = 0; c < length; c++)
            {
                buffer.WriteTemplateValue(profile, destructureValues: true, items[c]);

                if (options.ContentBetween != null && c != length - 1)
                {
                    buffer.Write(options.ContentBetween);
                }
            }

            if (options.ContentAfter != null)
            {
                buffer.Write(options.ContentAfter);
            }
        }
    }
}