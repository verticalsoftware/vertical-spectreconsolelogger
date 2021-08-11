namespace Vertical.SpectreLogger.PseudoTypes
{
    /// <summary>
    /// Defines a pseudo type that represents a null value.
    /// </summary>
    // ReSharper disable once ConvertToStaticClass
    public sealed class NullValue
    {
        public static readonly NullValue Default = new NullValue();
        
        private NullValue()
        {
        }

        /// <inheritdoc />
        public override string ToString() => "(null)";
    }
}