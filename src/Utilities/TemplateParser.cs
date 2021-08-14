using System.Collections.Generic;
using System.Text.RegularExpressions;
using Vertical.SpectreLogger.Core;

namespace Vertical.SpectreLogger.Utilities
{
    /// <summary>
    /// Parses a template string into component parts.
    /// </summary>
    public static class TemplateParser
    {
        public static ICollection<TemplateSpan> Split(string str)
        {
            const int defaultCapacity = 6;
            const string templatePattern = @"(?<!\{)\{([^}]+)\}";
            
            var position = 0;
            var spans = new List<TemplateSpan>(defaultCapacity);

            for (var match = Regex.Match(str, templatePattern); match.Success; match = match.NextMatch())
            {
                if (match.Index > position)
                {
                    spans.Add(new TemplateSpan(str, position, match.Index-position));
                }
                
                spans.Add(new TemplateSpan(str, match.Index, match.Length, match));
                position = match.Index + match.Length;
            }

            if (position < str.Length - 1)
            {
                spans.Add(new TemplateSpan(str, position, str.Length-position));
            }
            
            return spans;
        }
    }
}