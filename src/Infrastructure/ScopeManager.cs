using System;
using System.Threading;

namespace Vertical.SpectreLogger.Infrastructure
{
    internal class ScopeManager : IScopeManager
    {
        private readonly AsyncLocal<IScopeProvider?> _localScopeHead = new();

        /// <inheritdoc />
        public object?[] Scopes
        {
            get
            {
                var head = _localScopeHead.Value;

                if (head == null)
                {
                    return Array.Empty<object?>();
                }

                var values = new object?[head.Iteration + 1];

                for (; head != null; head = head.PreviousScope)
                {
                    values[head.Iteration] = head.State;
                }

                return values;
            }
        }

        /// <inheritdoc />
        public IDisposable BeginScope<TState>(TState state)
        {
            var head = _localScopeHead.Value;
            var provider = new ScopeProvider(this, head, (head?.Iteration ?? -1) + 1, state);
            _localScopeHead.Value = provider;
            
            return provider;
        }

        /// <inheritdoc />
        public void EndScope(IScopeProvider scopeProvider)
        {
            var head = _localScopeHead.Value;

            while (head != null)
            {
                if (ReferenceEquals(head, scopeProvider))
                {
                    _localScopeHead.Value = head.PreviousScope;
                    return;
                }

                head = head.PreviousScope;
            }
            
            // All scopes disposed
            _localScopeHead.Value = null;
        }
    }
}