using System;

namespace Vertical.SpectreLogger.Formatting
{
    /// <summary>
    /// Represents a strongly typed formatter for values.
    /// </summary>
    /// <typeparam name="T">Value type</typeparam>
    internal sealed class ProviderFormatter<T> : ICustomFormatter where T : notnull
    {
        private readonly Func<string?, T, IFormatProvider?, string> _function;

        internal ProviderFormatter(Func<string?, T, IFormatProvider?, string> function) => _function = function
            ?? throw new ArgumentNullException(nameof(function));

        /// <inheritdoc />
        public string Format(string? format, object? arg, IFormatProvider? formatProvider)
        {
            return _function(format, (T) arg!, formatProvider);
        }
    }
}