using Microsoft.Extensions.Logging;
using Vertical.SpectreLogger.Core;

namespace Vertical.SpectreLogger.Infrastructure
{
    public class DefaultLogLevelController : ILogLevelController
    {
        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        /// <param name="minimumLevel">The minimum level.</param>
        public DefaultLogLevelController(LogLevel minimumLevel)
        {
            MinimumLevel = minimumLevel;
        }
        
        /// <inheritdoc />
        public LogLevel MinimumLevel { get; }
    }
}