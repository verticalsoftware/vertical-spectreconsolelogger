using System.Collections.Generic;
using System.Text.RegularExpressions;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Internal;
using Vertical.SpectreLogger.Reflection;
using Vertical.SpectreLogger.Rendering;

namespace Vertical.SpectreLogger.Templates
{
    internal class TemplateRendererBuilder : ITemplateRendererBuilder
    {
        private readonly IEnumerable<RendererDescriptor> _descriptors;

        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        /// <param name="descriptors">Descriptors.</param>
        public TemplateRendererBuilder(IEnumerable<RendererDescriptor> descriptors)
        {
            _descriptors = descriptors;
        }
        
        /// <inheritdoc />
        public IReadOnlyList<ITemplateRenderer> GetRenderers(string templateString)
        {
            var rendererList = new List<ITemplateRenderer>(16);
            
            TemplateString.Split(templateString, (in TemplateSegment segment) =>
            {
                rendererList.Add(SelectRenderer(segment));
            });

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

                var parameters = new List<object> {segment};

                if (segment.Match != null)
                {
                    parameters.Add(segment.Match);
                    parameters.Add(segment);
                }

                return (ITemplateRenderer)TypeActivator.CreateInstance(descriptor.ImplementationType, parameters);
            }

            return new StaticSpanRenderer(segment.Value);
        }
    }
}