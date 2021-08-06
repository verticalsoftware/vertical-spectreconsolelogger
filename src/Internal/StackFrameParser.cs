using System;
using System.Linq;
using System.Text.RegularExpressions;
using Vertical.SpectreLogger.Rendering;

namespace Vertical.SpectreLogger.Internal
{
    internal static class StackFrameParser
    {
        internal static StackFrameInfo Parse(string stackFrame)
        {
            const string pattern = @"at (?<method>[^(]+)\((?<params>[^)]+)?\)( in (?<path>([a-zA-Z]:)?[^:]+):line (?<lineno>\d+))?";

            var matchGroups = Regex.Match(stackFrame, pattern).Groups;

            try
            {
                return new StackFrameInfo(
                    matchGroups["method"].Value,
                    BuildParameters(matchGroups["params"].Value),
                    matchGroups["path"].Value,
                    matchGroups["lineno"].Success ? int.Parse(matchGroups["lineno"].Value) : 0);
            }
            catch (Exception exception)
            {
                Console.WriteLine(
                    "Failed to parse the following stack frame:" + Environment.NewLine +
                    stackFrame,
                    exception);
                
                throw;
            }
        }

        private static (string Type, string Name)[] BuildParameters(string value)
        {
            const string pattern = @"(?<type>[a-zA-Z0-9`&]+) (?<name>\w+)";

            return Regex
                .Matches(value, pattern)
                .Cast<Match>()
                .Select(match => (match.Groups["type"].Value, match.Groups["name"].Value))
                .ToArray();
        }
    }
}