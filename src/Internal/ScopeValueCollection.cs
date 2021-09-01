using System;
using System.Collections.Generic;

namespace Vertical.SpectreLogger.Internal
{
    internal class ScopeValueCollection
    {
        private readonly Lazy<IReadOnlyList<object?>> _lazyScopes;

        internal ScopeValueCollection(LoggerScope? headScope)
        {
            _lazyScopes = new Lazy<IReadOnlyList<object?>>(() => CreateScopeArray(headScope));
        }

        private static IReadOnlyList<object?> CreateScopeArray(LoggerScope? headScope)
        {
            var list = new List<object?>(16);

            for (var scope = headScope; scope != null; scope = scope.PreviousScope)
            {
                list.Add(scope.Value);
            }

            return list;
        }

        /// <inheritdoc />
        public override string ToString() => string.Join(" => ", _lazyScopes.Value);
    }
}