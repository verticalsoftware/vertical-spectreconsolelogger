using System;

namespace Vertical.SpectreLogger
{
    public class SpectreLoggerScope : IDisposable
    {
        private readonly SpectreLoggerProvider _provider;
        private bool _disposed = false;

        internal SpectreLoggerScope(SpectreLoggerProvider provider, object? state)
        {
            _provider = provider;
            State = state;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;
            
            _provider.ScopeDisposed(this);
        }
        
        /// <summary>
        /// Gets the object state.
        /// </summary>
        internal object? State { get; }
    }
}