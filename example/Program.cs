using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Vertical.SpectreLogger;

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
            var logger = LoggerFactory.Create(builder =>
            {
                builder.AddSpectreConsole(options =>
                {
                });
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
                    "This is an example of a {logLevel} message. Sample parameters:\n" +
                    "   Integers:    {short}, {int}, {long}\n" +
                    "   Reals:       {single}, {double}, {decimal}\n" +
                    "   Strings:     {string}, {char}\n" +
                    "   Boolean:     {true}, {false}\n" +
                    "   Temporal:    {dateTime:u} {dateTimeOffset} {timespan}\n" +
                    "   Identifiers: {guid}\n" +
                    "   Objects:     {object}\n" +
                    "   Tuples:      {tuple}\n" +
                    "   Null:        {null}",
                    logLevel,
                    (short)10, 20, 30L,
                    1.5f, 2.5d, 3.5m,
                    "Hello, world!", 'h',
                    true, false,
                    DateTime.Now, DateTimeOffset.Now, TimeSpan.FromMinutes(1),
                    Guid.NewGuid(),
                    new{ message="Hello World!" },
                    (x: 10, y: 20, z: 30),
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
