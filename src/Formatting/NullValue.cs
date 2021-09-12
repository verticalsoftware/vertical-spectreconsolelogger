using System;

namespace Vertical.SpectreLogger.Formatting
{
    /// <summary>
    /// Provides a special representation of null that can be used for formatting.
    /// </summary>
    public sealed class NullValue
    {
        private NullValue()
        {
        }

        /// <summary>
        /// Defines a default instance.
        /// </summary>
        public static readonly NullValue Default = new NullValue();

        /// <inheritdoc />
        public override string ToString() => string.Empty;
        
        /// <summary>
        /// Represents the formatter for this type.
        /// </summary>
        [TypeFormatter(typeof(NullValue))]
        public sealed class Formatter : ICustomFormatter
        {
            /// <inheritdoc />
            public string Format(string format, object arg, IFormatProvider formatProvider) => "(null)";
        }
    }
}