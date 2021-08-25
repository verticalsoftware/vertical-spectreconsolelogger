namespace Vertical.SpectreLogger.Types
{
    /// <summary>
    /// Represents a value that will be formatted for destructured output.
    /// </summary>
    /// <typeparam name="T">Value type</typeparam>
    public class DestructuredValue<T> : TypeWrapper<T> where T : notnull
    {
        /// <inheritdoc />
        public DestructuredValue(T value) : base(value)
        {
        }
    }
}