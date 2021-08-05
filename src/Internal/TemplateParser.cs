using System.Collections.Generic;
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
    }
}