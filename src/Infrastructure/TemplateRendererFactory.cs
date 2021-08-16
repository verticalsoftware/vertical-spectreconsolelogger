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
        public IReadOnlyList<ITemplateRenderer> CreatePipeline(string outputTemplate)
        {
            var patterns = _options
                .RendererTypes
                .ToDictionary(type => type, GetTemplatePattern);

            var renderers = new List<ITemplateRenderer>(6);

            renderers.AddRange(TemplateParser
                .Split(outputTemplate)
                .Select(span => GetRenderer(span, patterns))
                .ToArray());
            
            renderers.Add(new NewLineRenderer(onlyIfNotAtMargin: true));

            return renderers;
        }

        private static ITemplateRenderer GetRenderer(TemplateSpan span, Dictionary<Type, string> patterns)
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

                var constructors = entry.Key.GetConstructors();
                var templateContext = new TemplateContext(match);

                return TryCreateRenderer(constructors, templateContext)
                       ?? TryCreateRenderer(constructors, match)
                       ?? TryCreateRenderer(constructors)
                       ?? throw new InvalidOperationException($"Renderer type {type} does not contain a compatible constructor.");
            }

            return new StaticSpanRenderer(span.Value);
        }

        private static ITemplateRenderer? TryCreateRenderer<T>(ConstructorInfo[] constructors, T parameter)
            where T : notnull
        {
            var constructor = constructors.FirstOrDefault(ctor => ctor
                .GetParameters()
                .Count(param => param.ParameterType == typeof(T)) == 1);

            return constructor?.Invoke(new object[] {parameter}) as ITemplateRenderer;
        }

        private static ITemplateRenderer? TryCreateRenderer(ConstructorInfo[] constructors)
        {
            var constructor = constructors.FirstOrDefault(ctor => ctor.GetParameters().Length == 0);

            return constructor?.Invoke(Array.Empty<object>()) as ITemplateRenderer;
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