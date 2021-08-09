using System;
using Microsoft.Extensions.Logging;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Rendering;

namespace SpectreLoggerExample
{
    public class Scopes : IExample
    {
        /// <inheritdoc />
        public string Description => "Render log event scopes";

        /// <inheritdoc />
        public Action<SpectreLoggerOptions> Configuration => options =>
        {
            options.ConfigureProfile(LogLevel.Information, profile =>
            {
                profile.OutputTemplate = "{Timestamp:dd-MM-yy @hh:mm} {Scopes}{Message:NewLine?}";
                profile.ConfigureRenderer<ScopeValuesRenderer.Options>(opt =>
                {
                    opt.DefaultTypeStyle = "cyan1";
                    opt.Separator = " => ";
                });
            });
        };

        /// <inheritdoc />
        public void Demo(ILogger logger)
        {
            using (logger.BeginScope($"ConnectionID={Guid.NewGuid().ToString("N")[..8]}"))
            {
                using (logger.BeginScope($"RequestID={Guid.NewGuid():N}"))
                {
                    logger.LogInformation("Query database for matches");
                }
            }
        }
    }
}