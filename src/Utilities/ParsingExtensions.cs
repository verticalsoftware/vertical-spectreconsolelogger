using System;
using System.Text.RegularExpressions;

namespace Vertical.SpectreLogger.Utilities
{
    public static class ParsingExtensions
    {
        public static void SplitTemplate(this string str, 
            Action<(string token, bool isTemplate)> callback)
        {
            var match = Regex.Match(str, @"(?<!\{)\{([^}]+)\}");
            var index = 0;

            for (; match.Success; match = match.NextMatch())
            {
                if (match.Index > index)
                {
                    callback((str.Substring(index, match.Index - index), false));
                }

                callback((match.Value, true));

                index = match.Index + match.Length;
            }

            if (index < str.Length)
            {
                callback((str.Substring(index, str.Length - index), false));
            }
        }
    }
}