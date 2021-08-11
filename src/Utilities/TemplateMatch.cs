namespace Vertical.SpectreLogger.Utilities
{
    public readonly struct TemplateMatch
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="span">Span value</param>
        /// <param name="isTemplate">Whether the span is a template.</param>
        public TemplateMatch(string span, bool isTemplate)
        {
            Span = span;
            IsTemplate = isTemplate;
        }
        
        /// <summary>
        /// Gets the span value.
        /// </summary>
        public string Span { get; }
        
        /// <summary>
        /// Gets whether the span is a template match.
        /// </summary>
        public bool IsTemplate { get; }

        /// <inheritdoc />
        public override string ToString() => $"{(IsTemplate ? "template" : "non-template")}=\"{Span}\"";
    }
}