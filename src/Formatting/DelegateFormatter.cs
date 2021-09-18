using System;

namespace Vertical.SpectreLogger.Formatting
{
    internal sealed class ValueFormatter<T> : ICustomFormatter where T : notnull
    {
        private readonly Func<string?, T, string> _function;
        
        internal ValueFormatter(Func<string?, T, string> function) => _function = function;
        
        /// <inheritdoc />
        public string Format(string? format, object? arg, IFormatProvider? formatProvider)
        {
            return _function(format, (T) arg!);
        }
    }
    
    internal sealed class ProviderFormatter<T> : ICustomFormatter where T : notnull
    {
        private readonly Func<string?, T, IFormatProvider?, string> _function;

        internal ProviderFormatter(Func<string?, T, IFormatProvider?, string> function) => _function = function;

        /// <inheritdoc />
        public string Format(string? format, object? arg, IFormatProvider? formatProvider)
        {
            return _function(format, (T) arg!, formatProvider);
        }
    }
}