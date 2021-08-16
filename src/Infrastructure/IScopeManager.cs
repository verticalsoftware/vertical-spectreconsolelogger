using System;

namespace Vertical.SpectreLogger.Infrastructure
{
    public interface IScopeManager
    {
        object?[] Scopes { get; }
        
        IDisposable BeginScope<TState>(TState state);

        void EndScope(IScopeProvider scopeProvider);
    }
}