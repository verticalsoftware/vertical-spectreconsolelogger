using System;
using Vertical.SpectreLogger.Core;

namespace Vertical.SpectreLogger.Internal
{
    internal sealed class DelegateLogEventFilter : ILogEventFilter
    {
        private readonly LogEventFilterDelegate _filter;

        internal DelegateLogEventFilter(LogEventFilterDelegate filter)
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