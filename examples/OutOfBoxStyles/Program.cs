using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Vertical.SpectreLogger;
using Vertical.SpectreLogger.Options;

namespace OutOfBoxStyles
{
    class Program
    {
        static void Main(string[] args)
        {
            var style = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What console logging [italic yellow]style[/] would you like to see?")
                    .AddChoices("Default", "Serilog", "Microsoft"));

            switch (style)
            {
                case "Default":
                    Run(builder => { });
                    break;
                
                case "Microsoft":
                    Run(builder => builder.UseMicrosoftConsoleStyle());
                    break;
                
                case "Serilog":
                    Run(builder => builder.UseSerilogConsoleStyle());
                    break;
            }
        }

        static void Run(Action<SpectreLoggingBuilder> configure)
        {
            var logger = LoggerFactory.Create(builder =>
            {
                builder
                    .AddSpectreConsole(configure)
                    .SetMinimumLevel(LogLevel.Trace);
            }).CreateLogger<Profile>();
            
            var logLevels = new[]
            {
                LogLevel.Trace, 
                LogLevel.Debug, 
                LogLevel.Information, 
                LogLevel.Warning, 
                LogLevel.Error,
                LogLevel.Critical
            };
            
            foreach (var logLevel in logLevels)
            {
                logger.Log(
                    logLevel,
                    GetException(),
                    "This is an example of a {logLevel} message. Sample type output:\n" +
                    "   Integers:       {short}, {int}, {long}\n" +
                    "   Reals:          {single}, {double}, {decimal}\n" +
                    "   Strings:        {string}, chars: {char}\n" +
                    "   Boolean:        {true}, {false}\n" +
                    "   Temporal:       {dateTime:s} - {dateTimeOffset:s} - {timespan}\n" +
                    "   Identifiers:    {guid}\n" +
                    "   Objects:        {object}\n" +
                    "   Tuples:         {tuple}\n" +
                    "   Destructured:   {@destructured}\n" +
                    "   Null:           {null}",
                    logLevel.ToString(),
                    (short)10, 20, 30L,
                    1.5f, 2.5d, 3.5m,
                    "Hello, world!", 'h',
                    true, false,
                    DateTime.Now, DateTimeOffset.Now, TimeSpan.FromMinutes(1),
                    Guid.NewGuid(),
                    new{ message="Hello World!" },
                    (x: 10, y: 20, z: 30),
                    new { x=10, y=20, z=30 },
                    null);
            }
        }

        private static Exception GetException()
        {
            try
            {
                Task.Run(() => Array.Empty<string>().First()).Wait();
            }
            catch (Exception exception)
            {
                return exception;
            }

            return new Exception();
        }
    }
}
