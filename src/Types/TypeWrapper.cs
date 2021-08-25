namespace Vertical.SpectreLogger.Types
{
    public abstract class TypeWrapper<T> where T : notnull
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="value">Value.</param>
        protected TypeWrapper(T value)
        {
            Value = value;
        }
        
        /// <summary>
        /// Gets the value.
        /// </summary>
        public T Value { get; }

        /// <inheritdoc />
        public override string ToString() => Value.ToString() ?? string.Empty;

        /// <inheritdoc />
        public override int GetHashCode() => Value.GetHashCode();

        /// <summary>
        /// Determines if the underlying value of this instance is equal to another. 
        /// </summary>
        /// <param name="other">Other instance to compare.</param>
        /// <returns>Boolean</returns>
        public bool Equals(TypeWrapper<T> other) => Value.Equals(other.Value);
    }
}