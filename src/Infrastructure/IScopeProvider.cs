using System;

namespace Vertical.SpectreLogger.Infrastructure
{
    public interface IScopeProvider : IDisposable
    {
        IScopeProvider? PreviousScope { get; }
        int Iteration { get; }
        object? State { get; }
    }
}