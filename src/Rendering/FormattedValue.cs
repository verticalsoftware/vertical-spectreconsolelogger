using Vertical.SpectreLogger.Internal;
using Vertical.SpectreLogger.MatchableTypes;
using Vertical.SpectreLogger.Options;

namespace Vertical.SpectreLogger.Rendering
{
    public readonly struct FormattedValue
    {
        private FormattedValue(string? value, string? markup)
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

        public static FormattedValue CreateFromProfile(FormattingProfile profile,
            object? value)
        {
            var resolvedType = value?.GetType() ?? typeof(Null);
            var formatter = profile.ValueFormatters.GetValueOrDefault(resolvedType)
                            ??
                            profile.ValueFormatters.GetValueOrDefault(typeof(object));
            var formattedValue = formatter?.Invoke(value) ?? value?.ToString();

            var markup = profile.TypeStyles.GetValueOrDefault(resolvedType)
                         ??
                         profile.TypeStyles.GetValueOrDefault(typeof(object));

            return new FormattedValue(formattedValue, markup);
        }

        /// <inheritdoc />
        public override string ToString() => Value ?? string.Empty;
    }
}