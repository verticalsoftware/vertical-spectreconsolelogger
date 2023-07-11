using System.Text.RegularExpressions;

namespace Vertical.SpectreLogger.Templates
{
    /// <summary>
    /// Represents a segment of a template string.
    /// </summary>
    public sealed class TemplateSegment
    {
        /// <summary>
        /// Gets the destructuring capture group.
        /// </summary>
        public const string DestructuringGroup = "_ds";
        
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

        /// <summary>
        /// Gets a template designed to indicate destructuring.
        /// </summary>
        internal static readonly TemplateSegment DestructureTemplate = new(
            Regex.Match("@", $"(?<{DestructuringGroup}>@)"), "", 0, 0);
        
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
            IsTemplate = match?.Success ?? false;
            Value = match?.Value ?? Source.Substring(startIndex, length);
            var groups = match?.Groups;
            
            if (groups == null)
                return;
            
            HasDestructureSpecifier = groups[DestructuringGroup].Success;
            InnerTemplate = groups[InnerTemplateGroup].Value;
            Key = groups[KeyGroup].Value;
            ControlCode = groups[ControlGroup].Value;
            ControlCodeMatched = groups[ControlGroup].Success;
            CompositeFormatSpan = groups[CompositeFormatSpanGroup].Value;
            AlignmentSpan = groups[WidthSpanGroup].Value;
            Alignment = int.TryParse(groups[WidthValueGroup].Value, out var i)
                ? i
                : null;
            FormatSpan = groups[FormatSpanGroup].Value;
            Format = groups[FormatValueGroup].Value;
        }

        /// <summary>
        /// Gets the match object or null if the segment is not a template.
        /// </summary>
        public Match? Match { get; }

        /// <summary>
        /// Gets whether the segment is a template.
        /// </summary>
        public bool IsTemplate { get; }

        /// <summary>
        /// Gets the segment value.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Gets whether the destructure operator was specified.
        /// </summary>
        public bool HasDestructureSpecifier { get; }

        /// <summary>
        /// Gets the inner content of the template with the braces removed.
        /// </summary>
        public string? InnerTemplate { get; }
        
        /// <summary>
        /// Gets the template key.
        /// </summary>
        public string? Key { get; }

        /// <summary>
        /// Gets the template control code.
        /// </summary>
        public string? ControlCode { get; }

        /// <summary>
        /// Gets whether the control code group was matched.
        /// </summary>
        public bool ControlCodeMatched { get; }
        
        /// <summary>
        /// Gets the format group value.
        /// </summary>
        public string? CompositeFormatSpan { get; }
        
        /// <summary>
        /// Gets the width span.
        /// </summary>
        public string? AlignmentSpan { get; }
        
        /// <summary>
        /// Gets the formatted width value, or null if the value is not available.
        /// </summary>
        public int? Alignment { get; }
        
        /// <summary>
        /// Gets the format span.
        /// </summary>
        public string? FormatSpan { get; }

        /// <summary>
        /// Gets the format value.
        /// </summary>
        public string? Format { get; }
        
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