using System;
using System.Text.RegularExpressions;

namespace Vertical.SpectreLogger.Templates
{
    public class Template
    {
        private readonly Lazy<string> _lazyPattern;

        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        public Template()
        {
            _lazyPattern = new Lazy<string>(() => TemplatePatternBuilder.BuildPattern(this));
        }
        
        /// <summary>
        /// Gets whether the template supports new line control.
        /// </summary>
        public bool NewLineControl { get; set; }
        
        /// <summary>
        /// Gets whether the template supports margin control.
        /// </summary>
        public bool MarginControl { get; set; }
        
        /// <summary>
        /// Gets whether the template supports field width formatting.
        /// </summary>
        public bool FieldWidthFormatting { get; set; }
        
        /// <summary>
        /// Gets whether the template supports composite formatting.
        /// </summary>
        public bool CompositeFormatting { get; set; }
        
        /// <summary>
        /// Gets the custom format.
        /// </summary>
        public string? CustomFormat { get; set; }
        
        /// <summary>
        /// Gets the custom pattern.
        /// </summary>
        public string? CustomPattern { get; set; }

        /// <summary>
        /// Gets the renderer key.
        /// </summary>
        public string? RendererKey { get; set; }

        /// <summary>
        /// Gets the match pattern for the template.
        /// </summary>
        public string Pattern => _lazyPattern.Value;

        /// <summary>
        /// Parses a template string to a <see cref="TemplateContext"/>
        /// </summary>
        /// <param name="template">Template to parse.</param>
        /// <returns><see cref="TemplateContext"/></returns>
        public TemplateContext Parse(string template)
        {
            var match = Regex.Match(template, Pattern);

            var newLineMode = match.Groups[CaptureGroups.NewLineAtMargin].Success
                ? NewLineMode.InsertAtMargin
                : match.Groups[CaptureGroups.NewLine].Success
                    ? NewLineMode.Insert
                    : NewLineMode.None;

            var margin = match.Groups[CaptureGroups.Margin].Success
                ? (int?)int.Parse(match.Groups[CaptureGroups.Margin].Value)
                : null;

            var marginMode = margin.HasValue
                ? match.Groups[CaptureGroups.MarginSet].Success ? MarginControlMode.Set : MarginControlMode.Offset
                : MarginControlMode.None;

            var marginAdjustment = match.Groups[CaptureGroups.Margin].Success
                ? int.Parse(match.Groups[CaptureGroups.Margin].Value)
                : 0;

            var fieldWidth = match.Groups[CaptureGroups.FieldWidth].Success
                ? (int?)int.Parse(match.Groups[CaptureGroups.FieldWidth].Value)
                : null;

            return new TemplateContext(
                match,
                CustomPattern,
                RendererKey,
                newLineMode,
                marginMode,
                marginAdjustment,
                fieldWidth,
                match.Groups[CaptureGroups.CompositeFormat].Value,
                match.Groups[CaptureGroups.CustomFormat].Value);
        }
    }
}