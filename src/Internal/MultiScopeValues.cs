using System;
using System.Collections.Generic;
using Vertical.SpectreLogger.Core;

namespace Vertical.SpectreLogger.Internal
{
    internal sealed class MultiScopeValues : IScopeValues
    {
        private readonly Lazy<IReadOnlyList<object>> _lazyCollection;

        internal MultiScopeValues(LoggerScope scope)
        {
            _lazyCollection = new(() =>
            {
                var list = new Stack<object>(5);
                var current = (LoggerScope?) scope;

                for (; current != null; current = current.PreviousScope)
                {
                    list.Push(current.Value);
                }

                return list.ToArray();
            });
        }

        /// <inheritdoc />
        public bool HasValues => true;

        /// <inheritdoc />
        public IReadOnlyList<object> Values => _lazyCollection.Value;

        /// <inheritdoc />
        public override string ToString() => string.Join(" => ", Values);
    }
}