using Microsoft.Extensions.Logging;
using Shouldly;
using Vertical.SpectreLogger.Tests.Infrastructure;
using Xunit;

namespace Vertical.SpectreLogger.Tests
{
    public class SpectreLoggerTests
    {
        [Fact]
        public void LoggerDoesNotOutputWhenLevelNotEnabled()
        {
            RendererTestHarness.Capture(
                    cfg => cfg.SetMinimumLevel(LogLevel.Information),
                    logger => logger.LogDebug("test"))
                .ShouldBeEmpty();
        }

        [Fact]
        public void LoggerOutputsWhenLevelIsEnabled()
        {
            RendererTestHarness.Capture(
                    cfg => cfg
                        .SetMinimumLevel(LogLevel.Debug)
                        .ConfigureProfiles(pro =>
                        {
                            pro.DefaultLogValueStyle = null;
                            pro.OutputTemplate = "{Message}";
                        }),
                    logger => logger.LogDebug("test message"))
                .ShouldBe("test message");
        }

        [Fact]
        public void LoggerEmitsWhenEventIsAboveMinimumOverride()
        {
            RendererTestHarness.Capture(
                    cfg =>
                    {
                        cfg.SetMinimumLevel(LogLevel.Information);
                        cfg.SetMinimumLevel("Minimum", LogLevel.Warning);
                        cfg.ConfigureProfiles(profile => profile.OutputTemplate = "{Message}");
                    },
                    log => log.LogWarning("warning"),
                    "Minimum")
                .ShouldBe("warning");
        }

        [Fact]
        public void LoggerDoesNotEmitWhenEventIsBelowMinimumOverride()
        {
            RendererTestHarness.Capture(
                    cfg =>
                    {
                        cfg.SetMinimumLevel(LogLevel.Information);
                        cfg.SetMinimumLevel("Minimum", LogLevel.Warning);
                        cfg.ConfigureProfiles(profile => profile.OutputTemplate = "{Message}");
                    },
                    log => log.LogInformation("information"),
                    "Minimum")
                .ShouldBeEmpty();
        }

        [Fact]
        public void LoggerPreservesFormatStringWhenConfigured()
        {
            RendererTestHarness.Capture(
                cfg => cfg.ConfigureProfiles(profiles =>
                {
                    profiles.PreserveMarkupInFormatStrings = true;
                    profiles.OutputTemplate = "{Message}";
                }),
                log => log.LogInformation("Here is [yellow]yellow[/], and here is [green1]green[/]."),
                "Program")
                .ShouldBe("Here is [yellow]yellow[/], and here is [green1]green[/].");            
        }
    }
}