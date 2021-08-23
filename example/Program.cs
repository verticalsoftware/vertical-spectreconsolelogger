using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Vertical.SpectreLogger;
using Vertical.SpectreLogger.Infrastructure;

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
                    builder.AddSpectreConsole();
                    builder.SetMinimumLevel(LogLevel.Trace);
                })
                .CreateLogger<Program>();

            using var scope = logger.BeginScope("ConnectionId = {id}", Guid.NewGuid().ToString("N")[..8]);
            using var scope2 = logger.BeginScope(new List<KeyValuePair<string, object>>
            {
                new("ClientId", Guid.NewGuid().ToString("N")[..8]),
                new("Authenticated", true)
            });

            var logLevels = new[]
            {
                LogLevel.Trace,
                LogLevel.Debug,
                LogLevel.Information,
                LogLevel.Warning,
                LogLevel.Error,
                LogLevel.Critical
            };

            var exception = GetException();

            foreach (var logLevel in logLevels)
            {
                const string message =
                    "This is an example of a {logLevel} event.\n"
                    + "  booleans  :  {t}, {f}\n"
                    + "  integers  :  {i}\n"
                    + "  floats    :  {single}, {double}, {decimal}\n"
                    + "  strings   :  {str}\n"
                    + "  temporal  :  {dto}, {ts}\n"
                    + "  custom:   :  {ok}, {not-ok}\n"
                    + "  null      :  {nothing}";

                logger.Log(
                    logLevel,
                    new EventId(100, "event 100"),
                    exception,
                    message,
                    logLevel,
                    true, false,
                    5000,
                    1.5f, 2.5d, 3.14m,
                    "Hello logger",
                    DateTimeOffset.Now, TimeSpan.FromMinutes(1),
                    new Ok(),
                    new Failed(),
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
