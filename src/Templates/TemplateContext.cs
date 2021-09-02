using System;
using System.Text.RegularExpressions;

namespace Vertical.SpectreLogger.Templates
{
    /// <summary>
    /// Defines the various properties of a regular expression match to
    /// a renderer template.
    /// </summary>
    public readonly struct TemplateContext
    {
        /// <summary>
        /// Defines the group name that captures the renderer template key.
        /// </summary>
        public const string KeyGroup = "_k";
        
        /// <summary>
        /// Defines the group name that captures the composite formatting span.
        /// </summary>
        public const string CompositeFormatSpanGroup = "_cfmtspan";
        
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
        public const string FormatSpanGroup = "_fmtspan";
        
        /// <summary>
        /// Defines the group name that captures the format code value of the composite
        /// formatting span.
        /// </summary>
        public const string FormatValueGroup = "_fmt";

        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        /// <param name="match">The match object.</param>
        internal TemplateContext(Match match)
        {
            Match = match ?? throw new ArgumentNullException(nameof(match));
        }

        /// <summary>
        /// Gets the original match object.
        /// </summary>
        public Match Match { get; }

        /// <summary>
        /// Gets the context key.
        /// </summary>
        public string Key => Match.Groups[KeyGroup].Value;

        /// <summary>
        /// Gets the format group value.
        /// </summary>
        public string CompositeFormatSpan => Match.Groups[CompositeFormatSpanGroup].Value;

        /// <summary>
        /// Gets the width span.
        /// </summary>
        public string WidthSpan => Match.Groups[WidthSpanGroup].Value;

        /// <summary>
        /// Gets the formatted width value, or 0 if the value is not available.
        /// </summary>
        public int Width => int.TryParse(Match.Groups[WidthValueGroup].Value, out var i)
            ? i
            : 0;

        /// <summary>
        /// Gets the format span.
        /// </summary>
        public string FormatSpan => Match.Groups[FormatSpanGroup].Value;

        /// <summary>
        /// Gets the format value.
        /// </summary>
        public string Format => Match.Groups[FormatValueGroup].Value;

        /// <inheritdoc />
        public override string ToString() => Match.ToString();
    }
}