using System.Text.RegularExpressions;

namespace Vertical.SpectreLogger.Templates
{
    /// <summary>
    /// Represents a segment of a template string.
    /// </summary>
    public readonly struct TemplateSegment
    {
        internal TemplateSegment(
            Match? match,
            string source,
            int startIndex,
            int length)
        {
            Match = match;
            Source = source;
            StartIndex = startIndex;
            Length = length;
        }

        /// <summary>
        /// Gets the match object or null if the segment is not a template.
        /// </summary>
        public Match? Match { get; }

        /// <summary>
        /// Gets whether the segment is a template.
        /// </summary>
        public bool IsTemplate => Match?.Success == true;

        /// <summary>
        /// Gets the segment value.
        /// </summary>
        public string Value => Match?.Value ?? Source.Substring(StartIndex, Length);

        /// <summary>
        /// Gets the inner content of the template with the braces removed.
        /// </summary>
        public string? InnerTemplate => Match?.Groups["_template"].Value;
        
        /// <summary>
        /// Gets the complete source string.
        /// </summary>
        public string Source { get; }

        /// <summary>
        /// Gets the segment start index within the source string.
        /// </summary>
        public int StartIndex { get; }
        
        /// <summary>
        /// Gets the length of the segment.
        /// </summary>
        public int Length { get; }

        /// <inheritdoc />
        public override string ToString() => Value;
    }
}