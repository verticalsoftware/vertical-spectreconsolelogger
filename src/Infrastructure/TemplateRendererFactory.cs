using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Rendering;
using Vertical.SpectreLogger.Utilities;

// ReSharper disable ParameterTypeCanBeEnumerable.Local

namespace Vertical.SpectreLogger.Infrastructure
{
    internal class TemplateRendererFactory : ITemplateRendererFactory
    {
        private readonly SpectreLoggerOptions _options;

        public TemplateRendererFactory(IOptions<SpectreLoggerOptions> optionsProvider)
        {
            _options = optionsProvider.Value;
        }
        
        /// <inheritdoc />
        public IReadOnlyList<ITemplateRenderer> CreatePipeline(FormattingProfile profile)
        {
            var valueCache = new RenderedValueCache();
            
            var patterns = _options
                .RendererTypes
                .ToDictionary(type => type, GetTemplatePattern);

            var renderers = new List<ITemplateRenderer>(6);

            renderers.AddRange(TemplateParser
                .Split(profile.OutputTemplate)
                .Select(span => GetRenderer(span, patterns, valueCache))
                .ToArray());

            renderers.Add(new EndEventRenderer());

            return renderers;
        }

        private static ITemplateRenderer GetRenderer(TemplateSpan span, Dictionary<Type, string> patterns, RenderedValueCache valueCache)
        {
            if (!span.IsTemplate)
            {
                return new StaticSpanRenderer(span.Value);
            }

            foreach (var entry in patterns)
            {
                var type = entry.Key;
                var pattern = entry.Value;
                var match = Regex.Match(span.Value, pattern);

                if (!match.Success)
                    continue;

                var rendererType = entry.Key;

                return (ITemplateRenderer)DynamicActivator.CreateInstance(rendererType, new object[]
                {
                    new TemplateContext(match),
                    match,
                    valueCache
                });
            }

            return new StaticSpanRenderer(span.Value);
        }

        private static string GetTemplatePattern(Type type)
        {
            var templateAttribute = type.GetCustomAttribute<TemplateAttribute>();

            return templateAttribute?.TemplatePattern 
                   ??
                   throw new InvalidOperationException($"Type {type} is missing {nameof(TemplateAttribute)} and cannot be used.");
        }
    }
}