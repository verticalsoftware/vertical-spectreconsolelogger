using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Internal;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Reflection;
using Vertical.SpectreLogger.Templates;

namespace Vertical.SpectreLogger.Rendering
{
    internal class TemplateRendererBuilder : ITemplateRendererBuilder
    {
        private readonly IEnumerable<TemplateDescriptor> _descriptors;
        private readonly SpectreLoggerOptions _options;

        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        /// <param name="optionsProvider">Options provider.</param>
        /// <param name="descriptors">Descriptors.</param>
        public TemplateRendererBuilder(
            IOptions<SpectreLoggerOptions> optionsProvider,
            IEnumerable<TemplateDescriptor> descriptors)
        {
            _descriptors = descriptors;
            _options = optionsProvider.Value;
        }
        
        /// <inheritdoc />
        public IReadOnlyList<ITemplateRenderer> GetRenderers(string templateString)
        {
            var rendererList = new List<ITemplateRenderer>(16);
            
            TemplateString.Split(templateString, (in TemplateSegment segment) =>
            {
                rendererList.Add(SelectRenderer(segment));
            });
            
            rendererList.Add(EndEventRenderer.Default);

            return rendererList;
        }

        private ITemplateRenderer SelectRenderer(in TemplateSegment segment)
        {
            if (!segment.IsTemplate)
            {
                return new StaticSpanRenderer(segment.Value);
            }

            foreach (var descriptor in _descriptors)
            {
                var match = Regex.Match(segment.Value, descriptor.Template);
                
                if (!match.Success)
                    continue;

                var parameters = new List<object>
                {
                    new TemplateSegment(match, 
                        segment.Value, 
                        0,
                        segment.Value.Length),
                    match,
                    _options
                };
                
                return (ITemplateRenderer)TypeActivator.CreateInstance(descriptor.ImplementationType, parameters);
            }

            return new StaticSpanRenderer(segment.Value);
        }
    }
}