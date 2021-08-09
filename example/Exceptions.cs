using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Vertical.SpectreLogger.Options;

namespace SpectreLoggerExample
{
    public class Exceptions : IExample
    {
        /// <inheritdoc />
        public string Description => "Configure and display exceptions";

        /// <inheritdoc />
        public Action<SpectreLoggerOptions> Configuration => options =>
        {
            options.ConfigureProfiles(profile =>
            {
                profile.OutputTemplate = "{LogLevel}: {CategoryName} - {Message}{Exception:NewLine?@2}";
            }); 
        };

        /// <inheritdoc />
        public void Demo(ILogger logger)
        {
            try
            {
                var _ = new[] {1, 2, 3}.First(i => i == 0);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "An exception has occurred");
            }
        }
    }
}