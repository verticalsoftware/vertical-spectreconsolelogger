using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Vertical.SpectreLogger;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Rendering;

namespace Scopes
{
    class OK
    {
        internal static OK Default = new OK();

        /// <inheritdoc />
        public override string ToString() => "(OK)";
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            DemoSingleScope();
            DemoMultipleScopes();
        }

        private static void DemoSingleScope()
        {
            AnsiConsole.WriteLine("Log event with single scope in template:");

            var logger = LoggerFactory.Create(builder =>
                    builder.AddSpectreConsole(specLogger =>
                    {
                        specLogger.ConfigureProfile(LogLevel.Information,
                            profile =>
                            {
                                profile.OutputTemplate = "{LogLevel}/user={Scope=UserId}: {Message}";
                                profile.AddTypeStyle<OK>("[green]");
                            });
                    }))
                .CreateLogger<Program>();

            var user = new List<KeyValuePair<string, object>>{new KeyValuePair<string, object>("UserId", Guid.NewGuid().ToString("N")[^8..])};
            using var scope = logger.BeginScope(user);
            
            logger.LogInformation("{Result} User login successful", OK.Default);
        }

        private static void DemoMultipleScopes()
        {
            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine("Log event with multiple scopes");

            var logger = LoggerFactory.Create(builder =>
                    builder.AddSpectreConsole(specLogger =>
                    {
                        specLogger.ConfigureProfile(LogLevel.Information,
                            profile => profile
                                .AddTypeStyle<OK>("[green]")
                                // Default is '=>' between and after scopes
                                .ConfigureOptions<ScopeValuesRenderer.Options>(opt => opt.ContentAfter = opt.ContentBetween = " >> ")
                                .OutputTemplate = "{LogLevel}: {Scopes}{Message}");
                    }))
                .CreateLogger<Program>();

            using var scope1 = logger.BeginScope("user={UserId}", Guid.NewGuid().ToString("N")[^8..]);
            using var scope2 = logger.BeginScope("connection={ConnectionId}", Guid.NewGuid().ToString("N")[^12..]);
            
            logger.LogInformation("{Result} User login successful", OK.Default);
        }
    }
}
