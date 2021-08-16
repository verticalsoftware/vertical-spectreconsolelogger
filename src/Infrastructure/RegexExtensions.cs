using System;
using System.Text.RegularExpressions;

namespace Vertical.SpectreLogger.Infrastructure
{
    internal static class RegexExtensions
    {
        internal static string? TryGetGroup(this Match match, string group, string? defaultValue)
        {
            var capture = match.Groups[group];

            return capture.Success
                ? capture.Value
                : defaultValue;
        }
        
        internal static T TryGetGroup<T>(this Match match, string group, Func<string, T> converter, T defaultValue)
        {
            var capture = match.Groups[group];

            return capture.Success
                ? converter(capture.Value)
                : defaultValue;
        }
    }
}