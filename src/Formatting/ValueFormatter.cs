using System;

namespace Vertical.SpectreLogger.Formatting
{
    public class ValueFormatter<T> : IFormatter
    {
        private readonly Func<T, string> _function;

        public ValueFormatter(Func<T, string> function)
        {
            _function = function;
        }
        
        /// <inheritdoc />
        public string Format(string format, object value)
        {
            return _function((T) value);
        }
    }
}