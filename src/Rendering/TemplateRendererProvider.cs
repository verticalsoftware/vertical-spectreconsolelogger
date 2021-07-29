using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Vertical.SpectreLogger.Internal;
using Vertical.SpectreLogger.Options;

namespace Vertical.SpectreLogger.Rendering
{
    internal class TemplateRendererProvider : ITemplateRendererProvider
    {
        private readonly Dictionary<LogLevel, ITemplateRenderer[]> _rendererDictionary;
        
         public TemplateRendererProvider(IOptions<SpectreLoggerOptions> optionsProvider,
             IEnumerable<ITemplateRenderer> renderers)
         {
             _rendererDictionary = Build(optionsProvider.Value, renderers);
         }

         private static Dictionary<LogLevel, ITemplateRenderer[]> Build(
             SpectreLoggerOptions options, 
             IEnumerable<ITemplateRenderer> renderers)
         {
             return options
                 .FormattingProfiles
                 .Select(entry => (key: entry.Key, value: BuildRendererCollection(entry.Value, renderers)))
                 .ToDictionary(result => result.key, result => result.value);
         }

         private static ITemplateRenderer[] BuildRendererCollection(FormattingProfile profile, 
             IEnumerable<ITemplateRenderer> renderers)
         {
             var template = (profile.OutputTemplate ?? SpectreLoggerDefaults.OutputTemplate);
             var list = new List<ITemplateRenderer>();

             if (profile.BaseMarkup != null)
             {
                 // Preface all rendering with this markup
                 list.Add(new UnescapedSpanRenderer($"[{profile.BaseMarkup}]"));
             }

             foreach (var (token, isTemplate) in TemplateParser.Parse(template, preserveFormat: true))
             {
                 ITemplateRenderer? renderer;
                 
                 // ReSharper disable once PossibleMultipleEnumeration
                 if (isTemplate && (renderer = renderers.LastOrDefault(instance => Regex
                     .IsMatch(token, instance.Template))) != null)
                 {
                     list.Add(renderer);
                     continue;
                 }
                 
                 list.Add(new StaticSpanRenderer(token));
             }

             if (profile.BaseMarkup != null)
             {
                 list.Add(new UnescapedSpanRenderer("[/]"));
             }
             
             list.Add(NewLineRenderer.Default);

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