using System;
using System.Threading;
using Microsoft.Extensions.Logging;
using Vertical.SpectreLogger;
using Vertical.SpectreLogger.Formatting;
using Vertical.SpectreLogger.Options;

namespace Formatting
{
    [TypeFormatter(typeof(Thread))]
    class ThreadFormatter : ICustomFormatter
    {
        /// <inheritdoc />
        public string Format(string? format, object? arg, IFormatProvider? formatProvider)
        {
            throw new NotImplementedException();
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
                        specLogger.ConfigureProfiles(profile => profile.AddTypeFormatter<Thread>(new ThreadFormatter()));
                    });
                })
                .CreateLogger<Program>();
        }
    }
}
