using System.Collections.Generic;
using Vertical.SpectreLogger.Core;

namespace Vertical.SpectreLogger.Internal
{
    internal sealed class SingleScopeValue : IScopeValues
    {
        internal SingleScopeValue(LoggerScope scope)
        {
            Items = new[] {scope.Value};
        }

        /// <inheritdoc />
        public bool HasValues => true;

        /// <inheritdoc />
        public IReadOnlyList<object> Items { get; }

        /// <inheritdoc />
        public override string ToString() => Items[0]?.ToString() ?? string.Empty;
    }
}