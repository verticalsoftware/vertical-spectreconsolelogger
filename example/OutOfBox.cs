using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Vertical.SpectreLogger;

namespace SpectreLoggerExample
{
    [Demo("Demonstrates out-of-box styling and options")]
    public class OutOfBox
    {
        private readonly Exception _exception = new Func<Exception>(() =>
        {
            try
            {
                Array.Empty<int>().First();
            }
            catch (Exception exception)
            {
                return exception;
            }
            throw new InvalidOperationException();
        })();
        
        [Demo("Shows what (most) settings look like out-of-box")]
        public void WriteMessage()
        {
            var logger = LoggerFactory
                .Create(builder =>
                {
                    builder.AddSpectreConsole(options => options.MinimumLevel = LogLevel.Trace);
                    builder.SetMinimumLevel(LogLevel.Trace);
                })
                .CreateLogger<OutOfBox>();

            var levels = new[]
            {
                LogLevel.Trace,
                LogLevel.Debug,
                LogLevel.Information,
                LogLevel.Warning,
                LogLevel.Error,
                LogLevel.Critical
            };

            foreach (var level in levels)
            {
                logger.Log(level,
                    _exception,
                    "This is an example of event at the {level} level."
                    + Environment.NewLine
                    + "Here are some rendered values:" +
                    Environment.NewLine
                    + "  Numbers: {x}, {y}, {z}" +
                    Environment.NewLine
                    + "  Boolean: {t}, {f}" + Environment.NewLine
                    + "  Date:    {dto}" + Environment.NewLine
                    + "  Strings: {str}" + Environment.NewLine
                    + "  Null:    {null}" + Environment.NewLine
                    + "Here's an exception:",
                    level,
                    10, 20L, 30f,
                    true, false,
                    DateTimeOffset.Now,
                    "Hello vertical-spectreconsolelogger",
                    null);
                Console.WriteLine();
            }
        }
    }
}