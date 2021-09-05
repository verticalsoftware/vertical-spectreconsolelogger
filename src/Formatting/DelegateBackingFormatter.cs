using System;

namespace Vertical.SpectreLogger.Formatting
{
    internal class DelegateBackingFormatter : CustomFormatter
    {
        private readonly ValueFormatter _formatter;

        internal DelegateBackingFormatter(ValueFormatter formatter)
        {
            _formatter = formatter;
        }

        /// <inheritdoc />
        public override string Format(string? format, object? arg, IFormatProvider? formatProvider)
        {
            return arg == null 
                ? string.Empty 
                : _formatter(format, arg, formatProvider);
        }
    }
}