using System.Collections.Generic;

namespace Vertical.SpectreLogger.Formatting
{
    /// <summary>
    /// Base class for value wrappers.
    /// </summary>
    /// <typeparam name="T">Value type.</typeparam>
    public abstract class ValueWrapper<T> where T : notnull
    {
        private static readonly EqualityComparer<T> Comparer = EqualityComparer<T>.Default;

        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        /// <param name="value"></param>
        protected ValueWrapper(T value) => Value = value;
        
        /// <summary>
        /// Gets the value.
        /// </summary>
        public T Value { get; }

        /// <inheritdoc />
        public override string ToString() => Value.ToString() ?? string.Empty;

        /// <inheritdoc />
        public override int GetHashCode() => Value.GetHashCode();

        /// <inheritdoc />
        public override bool Equals(object? obj) => obj is ValueWrapper<T> other
                                                    && Comparer.Equals(Value, other.Value);
    }
}