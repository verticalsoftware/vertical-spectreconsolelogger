using System;

namespace Vertical.SpectreLogger.Internal
{
    internal sealed class LoggerScope : IDisposable
    {
        private readonly ScopeManager _scopeManager;
        private bool _disposed;

        internal LoggerScope(ScopeManager scopeManager,
            LoggerScope? previousScope,
            object value)
        {
            PreviousScope = previousScope;
            Value = value;
            _scopeManager = scopeManager;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;
            _scopeManager.ScopeDisposed(this);
        }

        /// <summary>
        /// Gets the previous scope.
        /// </summary>
        internal LoggerScope? PreviousScope { get; }

        /// <summary>
        /// Gets the scope value.
        /// </summary>
        internal object Value { get; }

        /// <inheritdoc />
        public override string ToString() => Value?.ToString() ?? "(null)";
    }
}