using System.Text.RegularExpressions;
using Vertical.SpectreLogger.Infrastructure;

namespace Vertical.SpectreLogger.Core
{
    /// <summary>
    /// Represents the match of a template.
    /// </summary>
    public class TemplateContext
    {
        internal TemplateContext(Match match)
        {
            Match = match;
            Width = match.TryGetGroup<int?>(TemplatePatterns.WidthCaptureGroup, str => int.Parse(str), null);
            Format = match.TryGetGroup(TemplatePatterns.CompositeFormatCaptureGroup, null);
        }
        
        /// <summary>
        /// Gets the field width or null.
        /// </summary>
        public int? Width { get; }
        
        /// <summary>
        /// Gets the composite format or null.
        /// </summary>
        public string? Format { get; }

        /// <summary>
        /// Gets the match object.
        /// </summary>
        public Match Match { get; }

        /// <inheritdoc />
        public override string ToString() => Match.ToString();
    }
}