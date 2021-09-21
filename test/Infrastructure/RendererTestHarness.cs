using System;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Output;

namespace Vertical.SpectreLogger.Tests.Infrastructure
{
    public static class RendererTestHarness
    {
        internal static string Capture(
            Action<SpectreLoggingBuilder> configure,
            Action<ILogger> log,
            string? loggerName = null)
        {
            var builder = new StringBuilder();
            var consoleWriter = Substitute.For<IConsoleWriter>();
            consoleWriter
                .When(w => w.Write(Arg.Any<string>()))
                .Do(callInfo => builder.Append((string)callInfo.Args()[0]));
            var buffer = new WriteBuffer(consoleWriter);
            var logger = LoggerFactory.Create(logging =>
                    logging
                        .SetMinimumLevel(LogLevel.Trace)
                        .AddSpectreConsole(opt =>
                        {
                            opt.Services.AddSingleton<IWriteBuffer>(buffer);
                            configure(opt);
                        }))
                .CreateLogger(loggerName ?? "TestLogger");

            log(logger);

            return builder.ToString();
        }
        
        internal static void RunScenario(
            Action<SpectreLoggingBuilder> configure, 
            Action<ILogger> log,
            string expectedOutputPattern,
            string? loggerName = null)
        {
            Capture(configure, log, loggerName).ShouldMatch(expectedOutputPattern);
        }
        
        internal static void RunScenario(
            Action<SpectreLoggingBuilder> configure, 
            Action<ILogger> log,
            Action<string> validateOutput,
            string? loggerName = null)
        {
            validateOutput(Capture(configure, log, loggerName));
        }
    }
}