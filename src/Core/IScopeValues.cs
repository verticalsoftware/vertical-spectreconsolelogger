using System.Collections.Generic;

namespace Vertical.SpectreLogger.Core
{
    /// <summary>
    /// Provides access to scope values.
    /// </summary>
    public interface IScopeValues
    {
        /// <summary>
        /// Gets whether the collection has any values.
        /// </summary>
        bool HasValues { get; }
        
        /// <summary>
        /// Gets the items in the collection;
        /// </summary>
        IReadOnlyList<object> Values { get; }
    }
}