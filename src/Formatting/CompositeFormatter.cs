using System;

namespace Vertical.SpectreLogger.Formatting
{
    public class CompositeFormatter : IFormatter
    {
        private CompositeFormatter()
        {
        }

        /// <inheritdoc />
        public string Format(string format, object value)
        {
            var formatString = $"{{0{format}}}";

            return string.Format(formatString, value);
        }
    }

    public class CompositeFormatter<T> : IFormatter
    {
        private readonly Func<string, object, string> _function;

        public CompositeFormatter(Func<string, object, string> function)
        {
            _function = function;
        }

        /// <inheritdoc />
        public string Format(string format, object value)
        {
            return _function(format, (T) value);
        }
    }
}