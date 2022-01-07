using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Vertical.SpectreLogger.Internal
{
    /// <summary>
    /// Represents the info of a stack frame.
    /// </summary>
    internal readonly struct StackFrameInfo
    {
        private static readonly Regex CompiledRegex = new(
            @"(?:at )(?:(?<method>[\w\.<>`\[\]$]+)\((?:(?:,\s)?(?<type>[\w\[\]\.*&`]+)\s(?<name>[\w]+))*\))(?:(?: in )(?<file>.+)(?::line )(?<line>\d+))?",
            RegexOptions.Compiled);
            
        private readonly Match _match;

        private StackFrameInfo(Match match) => _match = match;

        /// <summary>
        /// Parses a stack frame.
        /// </summary>
        /// <param name="stackFrame">The stack frame to parse.</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool TryParse(string stackFrame, out StackFrameInfo value)
        {
            if (stackFrame == null)
            {
                throw new ArgumentNullException(stackFrame, nameof(stackFrame));
            }
            
            var match = CompiledRegex.Match(stackFrame);
            value = new StackFrameInfo(match);
            return match.Success;
        }

        /// <summary>
        /// Gets whether the internal regular expression was matched. If false, the input string
        /// was probably not stack frame data.
        /// </summary>
        public bool Matched => _match.Success;

        /// <summary>
        /// Gets the method name.
        /// </summary>
        public string Method => _match.Groups["method"].Value;

        /// <summary>
        /// Gets the parameter collection.
        /// </summary>
        public IEnumerable<(string type, string name)> Parameters
        {
            get
            {
                var names = _match.Groups["name"].Captures;
                var types = _match.Groups["type"].Captures;

                return Enumerable
                    .Range(0, names.Count)
                    .Select(index => (types[index].Value, names[index].Value));
            }
        }

        /// <summary>
        /// Gets the file name or null if the file name was not available in the original stack trace string.
        /// </summary>
        public string? File => _match.Groups["file"].Success ? _match.Groups["file"].Value : null;

        /// <summary>
        /// Gets the line number of null if the information was not available in the original stack trace string.
        /// </summary>
        public int? LineNumber => _match.Groups["line"].Success ? int.Parse(_match.Groups["line"].Value) : null;

        /// <inheritdoc />
        public override string ToString() => _match.Value;
    }
}