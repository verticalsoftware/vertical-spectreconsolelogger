using Microsoft.Extensions.Logging;

namespace Vertical.SpectreLogger.Core
{
    public interface ILogLevelController
    {
        /// <summary>
        /// Gets the minimum log level.
        /// </summary>
        LogLevel MinimumLevel { get; }
    }
}