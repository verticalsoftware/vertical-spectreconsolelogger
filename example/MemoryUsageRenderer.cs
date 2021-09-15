using System;
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
                var value = ((Value) arg)!.Value;
                var patternMatch = Regex.Match(format ?? string.Empty, @"(?<units>[BMG])(?<precision>\d+)?");
                var precision = patternMatch.Groups["precision"].Success
                    ? $":D{patternMatch.Groups["precision"].Value}"
                    : string.Empty;

                switch (patternMatch.Groups["units"].Value)
                {
                    case "M":
                        return string.Format("{0" + precision + "}MB", value/1000);
                    
                    case "G":
                        return string.Format("{0" + precision + "}GB", value / 1000000);
                    
                    default:
                        return $"{value}B";
                }
            }
        }

        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventContext context)
        {
            buffer.WriteLogValue(context.Profile, _template, new Value(Environment.WorkingSet));
        }
    }
}