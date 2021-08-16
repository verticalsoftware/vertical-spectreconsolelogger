namespace Vertical.SpectreLogger.Infrastructure
{
    internal sealed class ScopeProvider : IScopeProvider
    {
        private readonly IScopeManager _scopeManager;

        internal ScopeProvider(IScopeManager scopeManager,
            IScopeProvider? previousScope,
            int iteration,
            object? state)
        {
            PreviousScope = previousScope;
            Iteration = iteration;
            State = state;
            
            _scopeManager = scopeManager;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _scopeManager.EndScope(this);
        }

        public IScopeProvider? PreviousScope { get; }
        public int Iteration { get; }

        public object? State { get; }
    }
}