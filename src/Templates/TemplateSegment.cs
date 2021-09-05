using System.Text.RegularExpressions;

namespace Vertical.SpectreLogger.Templates
{
    /// <summary>
    /// Represents a segment of a template string.
    /// </summary>
    public sealed class TemplateSegment
    {
        /// <summary>
        /// Gets the inner template group.
        /// </summary>
        public const string InnerTemplateGroup = "_tmpl";
        
        /// <summary>
        /// Defines the group name that captures the renderer template key.
        /// </summary>
        public const string KeyGroup = "_key";

        /// <summary>
        /// Defines the group name that captures the control group.
        /// </summary>
        public const string ControlGroup = "_ctl";
        
        /// <summary>
        /// Defines the group name that captures the composite formatting span.
        /// </summary>
        public const string CompositeFormatSpanGroup = "_cfmt";
        
        /// <summary>
        /// Defines the group name that captures the width span portion of the composite
        /// formatting span.
        /// </summary>
        public const string WidthSpanGroup = "_wdspan";
        
        /// <summary>
        /// Defines the group name that captures the width value of the composite
        /// formatting span.
        /// </summary>
        public const string WidthValueGroup = "_wd";
        
        /// <summary>
        /// Defines the group name that captures the format span portion of the composite
        /// formatting span.
        /// </summary>
        public const string FormatSpanGroup = "_fmspan";
        
        /// <summary>
        /// Defines the group name that captures the format code value of the composite
        /// formatting span.
        /// </summary>
        public const string FormatValueGroup = "_fm";
        
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
        public string? InnerTemplate => Match?.Groups["_tmpl"].Value;
        
        /// <summary>
        /// Gets the template key.
        /// </summary>
        public string? Key => Match?.Groups[KeyGroup].Value;

        /// <summary>
        /// Gets the template control code.
        /// </summary>
        public string? ControlCode => Match?.Groups[ControlGroup].Value;
        
        /// <summary>
        /// Gets the format group value.
        /// </summary>
        public string? CompositeFormatSpan => Match?.Groups[CompositeFormatSpanGroup].Value;
        
        /// <summary>
        /// Gets the width span.
        /// </summary>
        public string? WidthSpan => Match?.Groups[WidthSpanGroup].Value;
        
        /// <summary>
        /// Gets the formatted width value, or 0 if the value is not available.
        /// </summary>
        public int? Width => int.TryParse(Match?.Groups[WidthValueGroup].Value, out var i)
            ? i
            : null;
        
        /// <summary>
        /// Gets the format span.
        /// </summary>
        public string? FormatSpan => Match?.Groups[FormatSpanGroup].Value;

        /// <summary>
        /// Gets the format value.
        /// </summary>
        public string? Format => Match?.Groups[FormatValueGroup].Value;
        
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