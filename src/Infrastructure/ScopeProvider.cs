using System;
using System.Collections.Immutable;

namespace Vertical.SpectreLogger.Infrastructure
{
    internal sealed class ScopeProvider : IDisposable
    {
        private readonly object _previousState;

        internal ScopeProvider(object previousState)
        {
            _previousState = previousState;
        }
        
        /// <inheritdoc />
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}