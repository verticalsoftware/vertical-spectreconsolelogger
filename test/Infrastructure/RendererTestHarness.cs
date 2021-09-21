using System;
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
            var buffer = new CapturingWriteBuffer(Substitute.For<IConsoleWriter>());
            var logger = LoggerFactory.Create(builder =>
                    builder
                        .SetMinimumLevel(LogLevel.Trace)
                        .AddSpectreConsole(opt =>
                    {
                        opt.Services.AddSingleton<IWriteBuffer>(buffer);
                        configure(opt);
                    }))
                .CreateLogger(loggerName ?? "TestLogger");

            log(logger);

            return buffer.ToString();
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