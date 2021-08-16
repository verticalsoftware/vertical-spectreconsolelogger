using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Vertical.SpectreLogger;

namespace SpectreLoggerExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = LoggerFactory.Create(builder =>
                {
                    builder.AddSpectreConsole();
                })
                .CreateLogger<Program>();
            
            logger.LogInformation("Nope");
        }
    }
}
