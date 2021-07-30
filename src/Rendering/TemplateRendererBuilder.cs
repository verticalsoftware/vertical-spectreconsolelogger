using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Vertical.SpectreLogger.Internal;
using Vertical.SpectreLogger.Options;

namespace Vertical.SpectreLogger.Rendering
{
    internal class TemplateRendererBuilder : ITemplateRendererBuilder
    {
        private readonly Dictionary<LogLevel, ITemplateRenderer[]> _rendererDictionary;
        
         public TemplateRendererBuilder(IOptions<SpectreLoggerOptions> optionsProvider,
             IEnumerable<TemplateDescriptor> descriptors)
         {
             _rendererDictionary = Build(optionsProvider.Value, descriptors);
         }

         private static Dictionary<LogLevel, ITemplateRenderer[]> Build(
             SpectreLoggerOptions options, 
             IEnumerable<TemplateDescriptor> descriptors)
         {
             return options
                 .FormattingProfiles
                 .Select(entry => (key: entry.Key, value: BuildRendererCollection(entry.Value, descriptors)))
                 .ToDictionary(result => result.key, result => result.value);
         }

         private static ITemplateRenderer[] BuildRendererCollection(FormattingProfile profile, 
             IEnumerable<TemplateDescriptor> descriptors)
         {
             var template = profile.OutputTemplate ?? SpectreLoggerDefaults.OutputTemplate;
             var list = new List<ITemplateRenderer>();

             if (profile.BaseMarkup != null)
             {
                 // Preface all rendering with this markup
                 list.Add(new UnescapedSpanRenderer($"[{profile.BaseMarkup}]"));
             }

             foreach (var (token, isTemplate) in TemplateParser.Parse(template, preserveFormat: true))
             {
                 TemplateDescriptor? selector = null;
                 
                 // ReSharper disable once PossibleMultipleEnumeration
                 if (isTemplate && (selector = descriptors.LastOrDefault(sel => sel.Select(token))) != null)
                 {
                     var renderer = selector.Create(token);
                     list.Add(renderer);
                     continue;
                 }
                 
                 list.Add(new StaticSpanRenderer(token));
             }

             if (profile.BaseMarkup != null)
             {
                 list.Add(new UnescapedSpanRenderer("[/]"));
             }
             
             list.Add(new NewLineRenderer());

             return list.ToArray();
         }

         /// <inheritdoc />
         public IEnumerable<ITemplateRenderer> GetRenderers(LogLevel logLevel)
         {
             return _rendererDictionary.TryGetValue(logLevel, out var renderers)
                 ? renderers
                 : Array.Empty<ITemplateRenderer>();
         }
     }
}