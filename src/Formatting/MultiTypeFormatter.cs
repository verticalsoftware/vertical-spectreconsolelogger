using System;
using System.Collections.Generic;
using System.Globalization;

namespace Vertical.SpectreLogger.Formatting
{
    /// <summary>
    /// Represents a <see cref="ICustomFormatter"/> that uses the profile type
    /// formatters.
    /// </summary>
    internal class MultiTypeFormatter : ICustomFormatter
    {
        private readonly Dictionary<Type, ICustomFormatter> _typeFormatters;

        internal MultiTypeFormatter(Dictionary<Type, ICustomFormatter> typeFormatters)
        {
            _typeFormatters = typeFormatters ?? throw new ArgumentNullException(nameof(typeFormatters));
        }

        /// <inheritdoc />
        public string Format(string? format, object? arg, IFormatProvider? formatProvider)
        {
            if (arg == null)
            {
                return string.Empty;
            }

            if (_typeFormatters.TryGetValue(arg.GetType(), out var formatter))
                return formatter.Format(format, arg, formatProvider);

            if (arg is IFormattable formattableValue)
                return formattableValue.ToString(format, CultureInfo.CurrentCulture);

            return arg.ToString() ?? string.Empty;
        }
    }
}