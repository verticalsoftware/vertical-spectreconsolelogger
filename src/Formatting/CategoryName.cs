namespace Vertical.SpectreLogger.Formatting
{
    /// <summary>
    /// Pseudo-type for category name.
    /// </summary>
    public sealed class CategoryName
    {
        internal CategoryName(string value)
        {
            Value = value;
        }
        
        /// <summary>
        /// Gets the category name.
        /// </summary>
        public string Value { get; }

        /// <inheritdoc />
        public override string ToString() => Value;

        /// <inheritdoc />
        public override int GetHashCode() => Value.GetHashCode();
    }
}