using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Vertical.SpectreLogger.Internal
{
    internal static class TemplateParser
    {
        internal static IEnumerable<(string token, bool isTemplate)> Parse(string str, bool preserveFormat = true)
        {
            var match = Regex.Match(str, @"(?<!\{)\{([^}]+)\}");
            var index = 0;
            var group = preserveFormat ? 0 : 1;

            for (; match.Success; match = match.NextMatch())
            {
                if (match.Index > index)
                {
                    yield return (str.Substring(index, match.Index-index), false);
                }

                var token = match.Groups[group].Value;
                
                yield return (token, true);

                index = match.Index + match.Length;
            }

            if (index < str.Length)
            {
                yield return (str.Substring(index, str.Length - index), false);
            }
        }
        
        internal static void GetTokens(string str, Action<Match?, string> callback)
        {
            var match = Regex.Match(str, @"(?<!\{)\{([^}]+)\}");
            var index = 0;

            for (; match.Success; match = match.NextMatch())
            {
                if (match.Index > index)
                {
                    callback(null, str.Substring(index, match.Index - index));
                }

                var templateId = match.Groups[1].Value;
                callback(match, templateId);

                index = match.Index + match.Length;
            }

            if (index < str.Length)
            {
                callback(null, str.Substring(index, str.Length - index));
            }
        }
        
        internal static IEnumerable<Match> ParseMatches(string str)
        {
            return Regex.Matches(str, @"(?<!\{)\{([^}]+)\}").Cast<Match>();
        }
    }
}