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
            _typeFormatters = typeFormatters;
        }

        /// <inheritdoc />
        public string Format(string? format, object? arg, IFormatProvider? formatProvider)
        {
            if (arg == null)
            {
                return string.Empty;
            }

            return _typeFormatters.TryGetValue(arg.GetType(), out var formatter)
                ? formatter.Format(format, arg, formatProvider)
                : FormatCore(format, arg);
        }

        private static string FormatCore(string? format, object arg)
        {
            return arg is IFormattable formattableValue
                ? formattableValue.ToString(format, CultureInfo.CurrentCulture)
                : arg.ToString() ?? string.Empty;
        }
    }
}