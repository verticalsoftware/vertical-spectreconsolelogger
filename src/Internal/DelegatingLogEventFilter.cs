using System;
using Vertical.SpectreLogger.Core;

namespace Vertical.SpectreLogger.Internal
{
    internal sealed class DelegatingLogEventFilter : ILogEventFilter
    {
        private readonly LogEventFilterDelegate _filter;

        internal DelegatingLogEventFilter(LogEventFilterDelegate filter)
        {
            _filter = filter ?? throw new ArgumentNullException(nameof(filter));
        }
        
        /// <inheritdoc />
        public bool Filter(in LogEventContext eventContext)
        {
            return _filter(eventContext);
        }
    }
}