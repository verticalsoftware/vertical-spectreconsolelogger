using System;
using System.Text;

namespace Vertical.SpectreLogger.Templates
{
    public static class TemplatePatternBuilder
    {
        /// <summary>
        /// Creates a new template parser.
        /// </summary>
        /// <returns>A <see cref="TemplateParser"/> instance.</returns>
        public static string BuildPattern(TemplateParser parser)
        {
            if (parser.CustomPattern != null)
            {
                return parser.CustomPattern;
            }
            
            var pattern = new StringBuilder(120);

            pattern.Append("{");
            pattern.Append(parser.RendererKey ?? throw new InvalidOperationException("Template parser has no renderer key or custom pattern"));

            if (parser.NewLineControl)
            {
                AppendCaptureGroup(pattern, CaptureGroups.NewLine, ":NewLine",
                    pb => AppendCaptureGroup(pb, CaptureGroups.NewLineAtMargin, @"\?", null, optional: true),
                    optional: true);
            }

            if (parser.MarginControl)
            {
                AppendCaptureGroup(pattern, id: null, "@",
                    pb =>
                    {
                        AppendCaptureGroup(pb, CaptureGroups.Margin, "-?\\d+", null, optional: false);
                        AppendCaptureGroup(pb, CaptureGroups.MarginSet, "!", null, optional: true);
                    },
                    optional: true);
            }

            if (parser.FieldWidthFormatting)
            {
                AppendCaptureGroup(pattern, CaptureGroups.FieldWidth, @"-?\d+", null, optional: true);
            }

            if (parser.CompositeFormatting)
            {
                AppendCaptureGroup(pattern, CaptureGroups.CompositeFormat, @"[^}]+", null, optional: true);
            }
            
            if (!string.IsNullOrWhiteSpace(parser.CustomFormat))
            {
                AppendCaptureGroup(pattern, CaptureGroups.CustomFormat, parser.CustomFormat!, null, optional: true);
            }

            pattern.Append("}");

            return pattern.ToString();
        }

        private static void AppendCaptureGroup(StringBuilder builder,
            string? id,
            string pattern,
            Action<StringBuilder>? configureInner,
            bool optional)
        {
            if (id != null)
            {
                builder.Append("(?<");
                builder.Append(id);
                builder.Append(">");
            }
            else builder.Append("(?:");
            builder.Append(pattern);
            configureInner?.Invoke(builder);
            builder.Append(")");
            if (optional)
            {
                builder.Append("?");
            }
        }
    }
}