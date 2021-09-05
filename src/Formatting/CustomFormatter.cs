using System;
using System.Globalization;

namespace Vertical.SpectreLogger.Formatting
{
    internal abstract class CustomFormatter : ICustomFormatter
    {
        /// <inheritdoc />
        public virtual string Format(string? format, object? arg, IFormatProvider? formatProvider)
        {
            if (arg == null)
            {
                return string.Empty;
            }

            if (arg is IFormattable formattableValue)
            {
                return formattableValue.ToString(format, CultureInfo.CurrentCulture);
            }

            return arg.ToString() ?? string.Empty;
        }
    }
}