using System.Text.RegularExpressions;

namespace Vertical.SpectreLogger.Templates
{
    public class TemplateContext
    {
        internal TemplateContext(
            Match matchContext,
            string? customPattern,
            string? rendererKey,
            NewLineMode newLineMode,
            MarginControlMode marginControlMode,
            int marginAdjustment,
            int? fieldWidth,
            string? compositeFormat,
            string? customFormat)
        {
            MatchContext = matchContext;
            CustomPattern = customPattern;
            RendererKey = rendererKey;
            NewLineMode = newLineMode;
            MarginControlMode = marginControlMode;
            MarginAdjustment = marginAdjustment;
            FieldWidth = fieldWidth;
            CompositeFormat = compositeFormat;
            CustomFormat = customFormat;
        }

        /// <summary>
        /// Gets the match context.
        /// </summary>
        public Match MatchContext { get; }

        public string? CustomPattern { get; }

        /// <summary>
        /// Gets the renderer key.
        /// </summary>
        public string? RendererKey { get; }
        
        /// <summary>
        /// Gets the new line mode.
        /// </summary>
        public NewLineMode NewLineMode { get; }
        
        /// <summary>
        /// Gets the margin control mode.
        /// </summary>
        public MarginControlMode MarginControlMode { get; }
        
        /// <summary>
        /// Gets the margin adjustment value.
        /// </summary>
        public int MarginAdjustment { get; }
        
        /// <summary>
        /// Gets the field width.
        /// </summary>
        public int? FieldWidth { get; }
        
        /// <summary>
        /// Gets the composite format.
        /// </summary>
        public string? CompositeFormat { get; }
        
        /// <summary>
        /// Gets the custom format.
        /// </summary>
        public string? CustomFormat { get; }
    }
}