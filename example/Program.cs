using System;
using System.Collections.Generic;
using System.IO;
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
                    options.ConfigureProfiles(profile =>
                        profile.OutputTemplate = "{LogLevel} {CategoryName}{NewLine:4}{Message}{Exception:NewLine}");
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

            if (args.Any(a => a == "colors"))
            {
                foreach (var property in typeof(Color).GetProperties().Where(p => p.PropertyType == typeof(Color)))
                {
                    var instance = (Color) property.GetValue(null)!;
                    AnsiConsole.MarkupLine($"[{instance.ToMarkup()}]Color = {property.Name}[/]");
                }
            }

            try
            {
                ThrowIt(new string[] { }, out var rv);
            }
            catch (Exception ex)
            {
                if (args.Any(a => a == "exception"))
                {
                    logger.LogError(ex, "So this just happened");
                }
            }
        }

        private static T ThrowIt<T>(IEnumerable<T> items, out KeyValuePair<string, T> returnValue)
        {
            throw new InvalidOperationException();
        }
    }
}
