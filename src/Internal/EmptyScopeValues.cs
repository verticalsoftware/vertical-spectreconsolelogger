using System;
using System.Collections.Generic;
using Vertical.SpectreLogger.Core;

namespace Vertical.SpectreLogger.Internal
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
        public IReadOnlyList<object> Items { get; } = Array.Empty<object>();

        /// <inheritdoc />
        public override string ToString() => "(empty)";
    }
}