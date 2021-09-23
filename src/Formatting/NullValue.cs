using System;

namespace Vertical.SpectreLogger.Formatting
{
    /// <summary>
    /// Provides a special representation of null that can be used for formatting.
    /// </summary>
    public sealed class NullValue : IFormattable
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

        /// <inheritdoc />
        public string ToString(string? format, IFormatProvider? formatProvider) => "(null)";
    }
}