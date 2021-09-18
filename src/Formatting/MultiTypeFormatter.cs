using System;
using System.Collections.Generic;

namespace Vertical.SpectreLogger.Formatting
{
    /// <summary>
    /// Represents a <see cref="ICustomFormatter"/> that uses the profile type
    /// formatters.
    /// </summary>
    internal class MultiTypeFormatter : CustomFormatter
    {
        private readonly Dictionary<Type, ICustomFormatter> _typeFormatters;

        internal MultiTypeFormatter(Dictionary<Type, ICustomFormatter> typeFormatters)
        {
            _typeFormatters = typeFormatters;
        }

        /// <inheritdoc />
        public override string Format(string? format, object? arg, IFormatProvider? formatProvider)
        {
            if (arg == null)
            {
                return string.Empty;
            }

            return _typeFormatters.TryGetValue(arg.GetType(), out var formatter)
                ? formatter.Format(format, arg, formatProvider)
                : base.Format(format, arg, formatProvider);
        }
    }
}