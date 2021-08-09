using System;
using Microsoft.Extensions.Logging;
using Vertical.SpectreLogger.Options;

namespace SpectreLoggerExample
{
    public interface IExample
    {
        string Description { get; }

        Action<SpectreLoggerOptions> Configuration { get; }

        void Demo(ILogger logger);
    }
}