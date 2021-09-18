using System.Collections.Generic;
using System.Linq;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Formatting;
using Vertical.SpectreLogger.Options;
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

            var printed = false;

            foreach (var scope in scopes.Values)
            {
                switch (scope)
                {
                    case IDictionary<string, object> dictionary:
                        printed = FastRenderDictionary(buffer, profile, dictionary);
                        break;
                    
                    case IReadOnlyList<KeyValuePair<string, object>> list:
                        printed = FastRenderList(buffer, profile, list);
                        break;
                    
                    case IEnumerable<KeyValuePair<string, object>> enumerable:
                        printed = RenderEnumerable(buffer, profile, enumerable);
                        break;
                }

                if (printed) { return; }
            }
        }

        private bool FastRenderDictionary(IWriteBuffer buffer, LogLevelProfile profile, IDictionary<string, object> dictionary)
        {
            if (!dictionary.TryGetValue(_scope, out var value))
                return false;
            
            buffer.WriteLogValue(profile, _template, value ?? NullValue.Default);
            return true;
        }

        private bool FastRenderList(IWriteBuffer buffer, LogLevelProfile profile, IReadOnlyList<KeyValuePair<string, object>> list)
        {
            var count = list.Count;

            for (var c = 0; c < count; c++)
            {
                var element = list[c];

                if (element.Key != _scope) 
                    continue;
                
                buffer.WriteLogValue(profile, _template, element.Value ?? NullValue.Default);
                return true;
            }

            return false;
        }

        private bool RenderEnumerable(IWriteBuffer buffer, LogLevelProfile profile, IEnumerable<KeyValuePair<string, object>> enumerable)
        {
            var entry = enumerable.FirstOrDefault(kv => kv.Key == _scope);

            if (string.IsNullOrWhiteSpace(entry.Key))
                return false;
                
            buffer.WriteLogValue(profile, _template, entry.Value ?? NullValue.Default);

            return true;
        }
    }
}