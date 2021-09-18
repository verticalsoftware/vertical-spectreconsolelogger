using System;
using System.Collections.Generic;
using Vertical.SpectreLogger.Core;

namespace Vertical.SpectreLogger.Scopes
{
    internal sealed class EmptyScopeValues : IScopeValues
    {
        internal static readonly IScopeValues Default = new EmptyScopeValues();
        
        private EmptyScopeValues()
        {
        }

        /// <inheritdoc />
        public bool HasValues => false;

        /// <inheritdoc />
        public IReadOnlyList<object> Values { get; } = Array.Empty<object>();

        /// <inheritdoc />
        public override string ToString() => "(empty)";
    }
}