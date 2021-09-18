using System.Collections.Generic;
using System.Linq;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Formatting;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.Templates;

namespace Vertical.SpectreLogger.Rendering
{
    /// <summary>
    /// Renders individual scope values.
    /// </summary>
    public class ScopeValueRenderer : ITemplateRenderer
    {
        private readonly TemplateSegment _template;
        private readonly string _scope;

        /// <summary>
        /// Defines the template for the renderer.
        /// </summary>
        [Template] 
        public static readonly string Template = TemplatePatternBuilder
            .ForKey(@"Scope=(?<_scope>[\d\w_]+)")
            .AddFormatting()
            .AddAlignment()
            .Build();

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="template">Matching template.</param>
        public ScopeValueRenderer(TemplateSegment template)
        {
            _template = template;
            _scope = template.Match!.Groups["_scope"].Value;
        }

        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventContext context)
        {
            var profile = context.Profile;
            var scopes = context.ScopeValues;

            if (!scopes.HasValues)
                return;

            foreach (var scope in scopes.Values)
            {
                if (scope is not IReadOnlyList<KeyValuePair<string, object>> logValues)
                    continue;

                var entry = logValues.FirstOrDefault(kv => kv.Key == _scope);

                if (string.IsNullOrWhiteSpace(entry.Key))
                    continue;
                
                buffer.WriteLogValue(profile, _template, entry.Value ?? NullValue.Default);
                break;
            }
        }
    }
}