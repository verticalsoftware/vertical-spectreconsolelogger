using System;
using System.Text;

namespace Vertical.SpectreLogger.Templates
{
    /// <summary>
    /// Used to build the expression pattern for templates.
    /// </summary>
    public class TemplatePatternBuilder
    {
        private readonly string _keyPattern;
        private string? _controlPattern;
        private bool _widthFormatting;
        private bool _valueFormatting;
        private bool _destructuring;

        internal static readonly string SplitPattern = new TemplatePatternBuilder("[^,:{}>]+")
            {
                _destructuring = true,
                _controlPattern = "[^,:{}]+",
                _widthFormatting = true,
                _valueFormatting = true
            }
            .Build();

        private TemplatePatternBuilder(string keyPattern)
        {
            _keyPattern = keyPattern ?? throw new ArgumentNullException(nameof(keyPattern));
        }

        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        /// <param name="keyPattern">Key</param>
        /// <exception cref="ArgumentException"><paramref name="keyPattern"/> contains an invalid character.</exception>
        public static TemplatePatternBuilder ForKey(string keyPattern)
        {
            return new(keyPattern);
        }

        /// <summary>
        /// Adds a match group for the destructuring specifier.
        /// </summary>
        /// <returns>A reference to this instance.</returns>
        public TemplatePatternBuilder AddDestructuring()
        {
            _destructuring = true;
            return this;
        }

        /// <summary>
        /// Adds a pattern for a control group.
        /// </summary>
        /// <param name="controlPattern">Pattern to allow in the control group.</param>
        /// <returns>A reference to this instance.</returns>
        /// <exception cref="ArgumentException"><paramref name="controlPattern"/> contains an invalid character.</exception>
        public TemplatePatternBuilder AddControlGroup(string controlPattern)
        {
            _controlPattern = controlPattern ?? throw new ArgumentNullException(nameof(controlPattern));
            
            if (controlPattern.IndexOfAny(new[] {',', ':', '{', '}', '>'}) != -1)
            {
                throw new ArgumentException(
                    "Control pattern cannot contain any of the follow: ',', ':', '{', or '}'",
                    nameof(controlPattern));
            }
            
            return this;
        }

        /// <summary>
        /// Adds a pattern for width formatting.
        /// </summary>
        /// <returns>A reference to this instance.</returns>
        public TemplatePatternBuilder AddAlignmentGroup()
        {
            _widthFormatting = true;
            return this;
        }

        /// <summary>
        /// Adds a pattern for value formatting.
        /// </summary>
        /// <returns>A reference to this instance.</returns>
        public TemplatePatternBuilder AddFormattingGroup()
        {
            _valueFormatting = true;
            return this;
        }

        /// <inheritdoc />
        public override string ToString() => Build();
        
        /// <summary>
        /// Gets the template.
        /// </summary>
        public string Build()
        {
            var builder = new StringBuilder(200);

            builder.Append("(?<!{){(?<");
            builder.Append(TemplateSegment.InnerTemplateGroup);
            builder.Append(">(?<");
            builder.Append(TemplateSegment.KeyGroup);
            builder.Append(">");

            if (_destructuring)
            {
                builder.Append("(?<");
                builder.Append(TemplateSegment.DestructuringGroup);
                builder.Append(">@)?");
            }
            
            builder.Append(_keyPattern);
            builder.Append(")");

            if (_controlPattern != null)
            {
                builder.Append("(?:>(?<");
                builder.Append(TemplateSegment.ControlGroup);
                builder.Append(">");
                builder.Append(_controlPattern);
                builder.Append("))?");
            }

            if (_widthFormatting || _valueFormatting)
            {
                builder.Append("(?<");
                builder.Append(TemplateSegment.CompositeFormatSpanGroup);
                builder.Append(">");

                if (_widthFormatting)
                {
                    builder.Append("(?<");
                    builder.Append(TemplateSegment.WidthSpanGroup);
                    builder.Append(">,(?<");
                    builder.Append(TemplateSegment.WidthValueGroup);
                    builder.Append(@">-?\d+))?");
                }

                if (_valueFormatting)
                {
                    builder.Append("(?<");
                    builder.Append(TemplateSegment.FormatSpanGroup);
                    builder.Append(">:(?<");
                    builder.Append(TemplateSegment.FormatValueGroup);
                    builder.Append(">[^}]+))?");
                }
                
                builder.Append(@")");
            }

            builder.Append(")}");

            return builder.ToString();
        }
    }
}