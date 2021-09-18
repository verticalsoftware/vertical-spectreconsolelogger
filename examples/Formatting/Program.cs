using System;
using System.Threading;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Vertical.SpectreLogger;
using Vertical.SpectreLogger.Formatting;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Rendering;

namespace Formatting
{
    class ThreadFormatter : ICustomFormatter
    {
        /// <inheritdoc />
        public string Format(string? format, object? arg, IFormatProvider? formatProvider)
        {
            var thread = ((ThreadIdRenderer.Value) arg!).Value;

            return $"{thread.ManagedThreadId}:priority={thread.Priority}";
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            var logger = LoggerFactory
                .Create(builder =>
                {
                    builder.AddSpectreConsole(specLogger =>
                    {
                        specLogger.ConfigureProfiles(profile =>
                        {
                            profile.AddTypeFormatter<ThreadIdRenderer.Value>(new ThreadFormatter());
                            profile.AddTypeStyle<ThreadIdRenderer.Value>($"[{Color.Pink1}]");
                            profile.OutputTemplate = "[[{LogLevel}/{ThreadId}]]: {CategoryName} - {Message}";
                        });
                    });
                })
                .CreateLogger<Program>();
            
            logger.LogInformation("Displaying the thread id");
        }
    }
}
