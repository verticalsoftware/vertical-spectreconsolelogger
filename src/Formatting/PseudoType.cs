namespace Vertical.SpectreLogger.Formatting
{
    /// <summary>
    /// Base class for logger pseudo-types (types that are strings, but need to be
    /// represented specifically for formatting).
    /// </summary>
    public abstract class PseudoType
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="value">Value to encapsulate</param>
        protected PseudoType(object value)
        {
            Value = value;
        }
        
        /// <summary>
        /// Gets the value
        /// </summary>
        public object Value { get; }

        /// <inheritdoc />
        public override string ToString() => Value.ToString() ?? string.Empty;

        /// <inheritdoc />
        public override int GetHashCode() => Value.GetHashCode();
    }
}