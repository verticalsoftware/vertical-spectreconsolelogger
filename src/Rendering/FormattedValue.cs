namespace Vertical.SpectreLogger.Rendering
{
    public readonly struct FormattedValue
    {
        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        /// <param name="value">The formatted value.</param>
        /// <param name="markup">The markup to apply when rendering.</param>
        public FormattedValue(string? value, string? markup)
        {
            Value = value;
            Markup = markup;
        }

        /// <summary>
        /// Gets the formatted value.
        /// </summary>
        public string? Value { get; }

        /// <summary>
        /// Gets the markup.
        /// </summary>
        public string? Markup { get; }

        /// <summary>
        /// Gets whether a value is present.
        /// </summary>
        public bool HasValue => Value != null;

        /// <inheritdoc />
        public override string ToString() => Value ?? string.Empty;
    }
}