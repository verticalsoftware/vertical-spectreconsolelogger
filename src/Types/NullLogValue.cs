namespace Vertical.SpectreLogger.Types
{
    /// <summary>
    /// Provides a special representation for null log values.
    /// </summary>
    public sealed class NullLogValue
    {
        /// <summary>
        /// Defines the single instance.
        /// </summary>
        public static readonly NullLogValue Default = new NullLogValue();
        
        private NullLogValue()
        {
        }
    }
}