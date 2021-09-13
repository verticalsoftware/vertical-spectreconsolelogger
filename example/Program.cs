using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Vertical.SpectreLogger;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Rendering;

namespace SpectreLoggerExample
{
    class Program
    {
        public class Ok
        {
            /// <inheritdoc />
            public override string ToString() => "OK";
        }

        public class Failed
        {
            /// <inheritdoc />
            public override string ToString() => "Failed";
        }

        public class Person
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int Age { get; set; }
        }
        
        static void Main(string[] args)
        {
            Console.Clear();
            
            var logger = LoggerFactory.Create(builder =>
            {
                builder
                    //.AddSerilog(new LoggerConfiguration().WriteTo.Console().CreateLogger())
                    //.AddConsole()
                    .AddSpectreConsole(options =>
                    {
                        options.ConfigureProfiles(profile =>
                        {
                            profile.OutputTemplate = "{LogLevel}: {CategoryName}{Margin=6}{NewLine}{Message}{NewLine+}{Exception}";
                        });
                        options.ConfigureProfile(LogLevel.Trace, profile => profile
                            .AddTypeFormatter<LogLevel>(((fmt, obj, provider) => "trce"))
                            .AddTypeStyle<LogLevel>("[grey35]"));
                        options.ConfigureProfile(LogLevel.Debug, profile => profile
                            .AddTypeFormatter<LogLevel>(((fmt, obj, provider) => "dbug"))
                            .AddTypeStyle<LogLevel>("[grey46]"));
                        options.ConfigureProfile(LogLevel.Information, profile => profile
                            .AddTypeFormatter<LogLevel>(((fmt, obj, provider) => "info"))
                            .AddTypeStyle<LogLevel>("[green]"));
                        options.ConfigureProfile(LogLevel.Warning, profile => profile
                            .AddTypeFormatter<LogLevel>(((fmt, obj, provider) => "warn"))
                            .AddTypeStyle<LogLevel>("[gold3_1]"));
                        options.ConfigureProfile(LogLevel.Error, profile => profile
                            .AddTypeFormatter<LogLevel>(((fmt, obj, provider) => "fail"))
                            .AddTypeStyle<LogLevel>("[red1]"));
                        options.ConfigureProfile(LogLevel.Critical, profile => profile
                            .AddTypeFormatter<LogLevel>(((fmt, obj, provider) => "crit"))
                            .AddTypeStyle<LogLevel>("[white on red1]"));
                    })
                    .SetMinimumLevel(LogLevel.Trace);
            }).CreateLogger<Profile>();
            
            using var scope1 = logger.BeginScope("ConnectionId: {id}", Guid.NewGuid().ToString("N")[..8]);
            using var scope2 = logger.BeginScope(new[] {new KeyValuePair<string, object>("UserId", "@vertical.com")});

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
                    "This is an example of a {logLevel} message. Sample parameters:\n" +
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
