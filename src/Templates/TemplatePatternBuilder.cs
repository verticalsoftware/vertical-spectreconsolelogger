using System;
using System.Text;

namespace Vertical.SpectreLogger.Templates
{
    public static class TemplatePatternBuilder
    {
        /// <summary>
        /// Creates a new template parser.
        /// </summary>
        /// <returns>A <see cref="Template"/> instance.</returns>
        public static string BuildPattern(Template descriptor)
        {
            if (descriptor.CustomPattern != null)
            {
                return descriptor.CustomPattern;
            }
            
            var pattern = new StringBuilder(120);

            pattern.Append("{");
            pattern.Append(descriptor.RendererKey ?? string.Empty);

            if (descriptor.NewLineControl)
            {
                pattern.Append($"(?<{CaptureGroups.NewLine}>:NewLine(?<{CaptureGroups.NewLineAtMargin}>\\?)?)?");
            }

            if (descriptor.MarginControl)
            {
                pattern.Append($"(:Margin@(?<{CaptureGroups.Margin}>-?\\d+)(?<{CaptureGroups.MarginSet}>!)?)?");
            }

            if (descriptor.FieldWidthFormatting)
            {
                pattern.Append($"(,(?<{CaptureGroups.FieldWidth}>-?\\d+))?");
            }

            if (descriptor.CompositeFormatting)
            {
                pattern.Append($"(:(?<{CaptureGroups.CompositeFormat}>[^}}]+))?");
            }
            
            if (!string.IsNullOrWhiteSpace(descriptor.CustomFormat))
            {
                AppendCaptureGroup(pattern, CaptureGroups.CustomFormat, descriptor.CustomFormat!, null, optional: true);
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
            else
            {
                builder.Append("(?:");
            }
            
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