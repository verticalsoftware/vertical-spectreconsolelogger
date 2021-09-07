using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Internal;
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
            .AddWidthFormatting()
            .AddValueFormatting()
            .Build();

        /// <summary>
        /// Options for <see cref="LogLevelRenderer"/>
        /// </summary>
        public sealed class Options
        {
            /// <summary>
            /// Gets the names to display for log levels.
            /// </summary>
            public Dictionary<LogLevel, string> LevelDisplayValues { get; } = new()
            {
                [LogLevel.Trace] = "Trc",
                [LogLevel.Debug] = "Dbg",
                [LogLevel.Information] = "Inf",
                [LogLevel.Warning] = "Wrn",
                [LogLevel.Error] = "Err",
                [LogLevel.Critical] = "Crt"
            };
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="template">The matching template segment.</param>
        public LogLevelRenderer(TemplateSegment template) => _template = template;

        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventContext context)
        {
            var displayValue = context
                .Profile
                .RendererOptions
                .GetOptions<Options>()
                .LevelDisplayValues
                .GetValueOrDefault(context.LogLevel, context.LogLevel.ToString()); 
                
            buffer.WriteLogValueFormat(context.Profile, _template, displayValue!);
        }
    }
}