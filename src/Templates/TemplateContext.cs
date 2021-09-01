using System;
using System.Text.RegularExpressions;

namespace Vertical.SpectreLogger.Templates
{
    public sealed class TemplateContext
    {
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
        public string Key => Match.Groups["_key"].Value;

        /// <summary>
        /// Gets the format group value.
        /// </summary>
        public string CompositeFormat => Match.Groups["_composite_format"].Value;

        /// <summary>
        /// Gets the width span.
        /// </summary>
        public string WidthSpan => Match.Groups["_width"].Value;

        /// <summary>
        /// Gets the formatted width value, or 0 if the value is not available.
        /// </summary>
        public int Width => int.TryParse(Match.Groups["_width_value"].Value, out var i)
            ? i
            : 0;

        /// <summary>
        /// Gets the format span.
        /// </summary>
        public string FormatSpan => Match.Groups["_format"].Value;

        /// <summary>
        /// Gets the format value.
        /// </summary>
        public string Format => Match.Groups["_format_value"].Value;

        /// <inheritdoc />
        public override string ToString() => Match.ToString();
    }
}