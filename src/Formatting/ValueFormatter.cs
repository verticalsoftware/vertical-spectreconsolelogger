using System;

namespace Vertical.SpectreLogger.Formatting
{
    /// <summary>
    /// Represents a strongly typed formatter for a value type.
    /// </summary>
    /// <typeparam name="T">Value type being formatted.</typeparam>
    internal sealed class ValueFormatter<T> : ICustomFormatter where T : notnull
    {
        private readonly Func<string?, T, string> _function;

        internal ValueFormatter(Func<string?, T, string> function) => _function = function
            ?? throw new ArgumentNullException(nameof(function));
        
        /// <inheritdoc />
        public string Format(string? format, object? arg, IFormatProvider? formatProvider)
        {
            return _function(format, (T) arg!);
        }
    }
}