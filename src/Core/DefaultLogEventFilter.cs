using Microsoft.Extensions.Options;
using Vertical.SpectreLogger.Options;

namespace Vertical.SpectreLogger.Core
{
    internal class DefaultLogEventFilter : ILogEventFilter
    {
        private readonly SpectreConsoleLoggerOptions _options;

        public DefaultLogEventFilter(IOptions<SpectreConsoleLoggerOptions> optionsProvider)
        {
            _options = optionsProvider.Value;
        }
        
        /// <inheritdoc />
        public bool Render(in LogEventInfo eventInfo)
        {
            return
                eventInfo.LogLevel >= _options.MinimumLevel
                &&
                (_options.EventFilter?.Invoke(eventInfo) ?? true);

        }
    }
}