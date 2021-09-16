using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Formatting;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.Templates;

namespace SpectreLoggerExample
{
    public class MemoryUsageRenderer : ITemplateRenderer
    {
        private readonly TemplateSegment _template;

        /// <summary>
        /// This builds a regular expression pattern the framework will use when
        /// parsing output templates.
        /// </summary>
        [Template] public static readonly string Template = TemplatePatternBuilder
            .ForKey("MemoryUsage")
            .AddAlignmentGroup()
            .AddFormattingGroup()
            .Build();

        /// <summary>
        /// Creates a new instance of this type
        /// </summary>
        /// <param name="template">The segment of the output template that matched this instance.</param>
        public MemoryUsageRenderer(TemplateSegment template)
        {
            _template = template;
        }

        /// <summary>
        /// Wrap a type for formatting.
        /// </summary>
        public class Value : ValueWrapper<long>
        {
            /// <inheritdoc />
            public Value(long workingSet) : base(workingSet)
            {
            }
        }

        [TypeFormatter(typeof(Value))]
        public class Formatter : ICustomFormatter
        {
            /// <inheritdoc />
            public string Format(string format, object arg, IFormatProvider formatProvider)
            {
                var patternMatch = Regex.Match(format ?? string.Empty, @"(?<units>[BMG])(?<precision>\d+)?");
                var precision = patternMatch.Groups["precision"].Value;
                var units = patternMatch.Groups["units"].Value;
                var value = ((Value) arg).Value;
                var convertedValue = units switch
                {
                    "M" => value * 0.0000009537,
                    "G" => value * 0.0000000009,
                    _ => value
                };

                return convertedValue.ToString($"F{precision}") + units;
            }
        }

        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventContext context)
        {
            buffer.WriteLogValue(context.Profile, _template, new Value(Process.GetCurrentProcess().PrivateMemorySize64));
        }
    }
}