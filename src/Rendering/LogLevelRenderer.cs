using System;
using Microsoft.Extensions.Logging;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Formatting;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.Templates;

namespace Vertical.SpectreLogger.Rendering
{
    /// <summary>
    /// Renders the log level.
    /// </summary>
    public class LogLevelRenderer : ITemplateRenderer
    {
        private readonly TemplateSegment _template;

        [Template] 
        public static readonly string Template = TemplatePatternBuilder
            .ForKey("LogLevel")
            .AddAlignment()
            .AddFormatting()
            .Build();

        [TypeFormatter(typeof(LogLevel))]
        public class Formatter : ICustomFormatter
        {
            /// <inheritdoc />
            public string Format(string? format, object? arg, IFormatProvider? formatProvider)
            {
                var logLevel = (LogLevel) (arg ?? LogLevel.None);

                return logLevel switch
                {
                    LogLevel.Trace => "Trce",
                    LogLevel.Debug => "Dbug",
                    LogLevel.Information => "Info",
                    LogLevel.Warning => "Warn",
                    LogLevel.Error => "Fail",
                    LogLevel.Critical => "Crit",
                    _ => string.Empty
                };
            }
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="template">The matching template segment.</param>
        public LogLevelRenderer(TemplateSegment template) => _template = template;

        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventContext context)
        {
            buffer.WriteLogValue(context.Profile, _template, context.LogLevel);
        }
    }
}