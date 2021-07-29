using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Vertical.SpectreLogger;
using Vertical.SpectreLogger.Options;

namespace SpectreLoggerExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddSpectreConsole(options =>
                {
                    options.MinimumLevel = LogLevel.Trace;
                    options.ConfigureProfiles(profile => profile.OutputTemplate = @"[{LogLevel}] {Message}{Exception:nl}");
                });

                builder.SetMinimumLevel(LogLevel.Trace);
            });

            var logger = loggerFactory.CreateLogger<Program>();

            logger.LogCritical("This is a {level} message", "Critical");
            logger.LogError("This is a {level} message", "Error");
            logger.LogWarning("This is a {level} message", "Warning");
            logger.LogInformation("This is a {level} message", "Information");
            logger.LogDebug("This is a {level} message", "Debug");
            logger.LogTrace("This is a {level} message", "Trace");

            if (args.Length == 1 && args[0] == "colors")
            {
                foreach (var property in typeof(Color).GetProperties().Where(p => p.PropertyType == typeof(Color)))
                {
                    var instance = (Color) property.GetValue(null)!;
                    AnsiConsole.MarkupLine($"[{instance.ToMarkup()}]Color = {property.Name}[/]");
                }
            }

            try
            {
                throw new InvalidOperationException();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "This is an error");
            }
        }
    }
}
