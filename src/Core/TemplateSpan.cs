using System.Text.RegularExpressions;

namespace Vertical.SpectreLogger.Core
{
    /// <summary>
    /// Defines a span of characters within an output template.
    /// </summary>
    public class TemplateSpan
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="source">Gets the source expression for the match.</param>
        /// <param name="length">The length of the match value.</param>
        /// <param name="match">The regular expression match object.</param>
        /// <param name="startIndex">The position in the source where the match begins.</param>
        public TemplateSpan(string source,
            int startIndex,
            int length,
            Match? match = null)
        {
            Source = source;
            StartIndex = startIndex;
            Length = length;
            Match = match;
        }

        /// <summary>
        /// Gets the source expression.
        /// </summary>
        public string Source { get; }
        
        /// <summary>
        /// Gets the position within the source.
        /// </summary>
        public int StartIndex { get; }

        /// <summary>
        /// Gets the length of the match value.
        /// </summary>
        public int Length { get; }

        public string Value => Match?.Value ?? Source.Substring(StartIndex, Length);

        /// <summary>
        /// Gets whether this instance represents a match to a template.
        /// </summary>
        public bool IsTemplate => Match != null;

        /// <summary>
        /// Gets the match object for a template span.
        /// </summary>
        public Match? Match { get; }

        /// <inheritdoc />
        public override string ToString() => $"{(IsTemplate ? "template" : "span")}=\"{Value}\" @{StartIndex}";
    }
}