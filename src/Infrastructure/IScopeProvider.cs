using System;

namespace Vertical.SpectreLogger.Infrastructure
{
    /// <summary>
    /// Provides a disposable object used to manage a scope.
    /// </summary>
    public interface IScopeProvider : IDisposable
    {
        /// <summary>
        /// Gets the previous scope.
        /// </summary>
        IScopeProvider? PreviousScope { get; }
        
        /// <summary>
        /// Gets the iteration.
        /// </summary>
        int Iteration { get; }
        
        /// <summary>
        /// Gets the state value.
        /// </summary>
        object? State { get; }
    }
}