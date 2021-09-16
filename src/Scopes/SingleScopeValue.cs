using System.Collections.Generic;
using Vertical.SpectreLogger.Core;

namespace Vertical.SpectreLogger.Scopes
{
    internal sealed class SingleScopeValue : IScopeValues
    {
        internal SingleScopeValue(LoggerScope scope)
        {
            Values = new[] {scope.Value};
        }

        /// <inheritdoc />
        public bool HasValues => true;

        /// <inheritdoc />
        public IReadOnlyList<object> Values { get; }

        /// <inheritdoc />
        public override string ToString() => Values[0]?.ToString() ?? string.Empty;
    }
}