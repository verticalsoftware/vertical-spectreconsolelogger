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
             var template = profile.OutputTemplate ?? SpectreLoggerOptions.OutputTemplate;
             var list = new List<ITemplateRenderer>();

             if (profile.BaseEventStyle != null)
             {
                 // Preface all rendering with this markup
                 list.Add(new UnescapedSpanRenderer($"[{profile.BaseEventStyle}]"));
             }
             
             TemplateParser.GetTokens(template, (match, token) =>
             {
                 switch (match)
                 {
                     case { }:
                         var renderer = TryGetRendererInstance(descriptors, match.Value) ;
                         list.Add(renderer ?? new FormattedLogValueRenderer(match.Value));
                         break;
                     
                     default:
                         list.Add(new StaticSpanRenderer(token));
                         break;
                 }
             });

             if (profile.BaseEventStyle != null)
             {
                 list.Add(new UnescapedSpanRenderer("[/]"));
             }
             
             list.Add(new EndEventRenderer());

             return list.ToArray();
         }

         private static ITemplateRenderer? TryGetRendererInstance(IEnumerable<TemplateDescriptor> descriptors, string matchValue)
         {
             foreach (var descriptor in descriptors)
             {
                 if (descriptor.TryCreateRenderer(matchValue, out var renderer))
                     return renderer!;
             }

             return null;
         }

         /// <inheritdoc />
         public ITemplateRenderer[] GetRenderers(LogLevel logLevel)
         {
             return _rendererDictionary.TryGetValue(logLevel, out var renderers)
                 ? renderers
                 : Array.Empty<ITemplateRenderer>();
         }
     }
}