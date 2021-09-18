using Vertical.SpectreLogger.Formatting;

namespace Vertical.SpectreLogger.Destructuring
{
    /// <summary>
    /// Wraps the key of a destructured value.
    /// </summary>
    public class DestructuredKeyValue : ValueWrapper<string>
    {
        /// <inheritdoc />
        public DestructuredKeyValue(string key) : base(key)
        {
        }
    }
}