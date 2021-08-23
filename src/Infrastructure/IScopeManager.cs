using System;

namespace Vertical.SpectreLogger.Infrastructure
{
    /// <summary>
    /// Manages scopes in the loggers.
    /// </summary>
    public interface IScopeManager
    {
        object?[] Scopes { get; }
        
        IDisposable BeginScope<TState>(TState state);

        void EndScope(IScopeProvider scopeProvider);
    }
}