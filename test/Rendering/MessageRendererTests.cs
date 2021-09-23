using Microsoft.Extensions.Logging;
using Shouldly;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Tests.Infrastructure;
using Xunit;

namespace Vertical.SpectreLogger.Tests.Rendering
{
    public class MessageRendererTests
    {
        [Fact]
        public void RenderWritesExpectedOutput()
        {
            RendererTestHarness.Capture(ConfigureSettings, 
                logger => logger.LogInformation("plain message"))
                .ShouldBe("plain message");
        }

        [Fact]
        public void RenderWritesExpectedOutputWithLogValues()
        {
            RendererTestHarness.Capture(ConfigureSettings,
                logger => logger.LogInformation("plain message {x} & {y}", 10, 20))
                .ShouldBe("plain message 10 & 20");
        }

        [Fact]
        public void RenderWritesExpectedOutputWithDestructuring()
        {
            RendererTestHarness.Capture(ConfigureSettings,
                    logger => logger.LogInformation("plain message {@obj}", 
                        new{ Name="Testy McTesterson", Age=30 }))
                .ShouldBe("plain message {Name: Testy McTesterson, Age: 30}");
        }

        private static void ConfigureSettings(SpectreLoggingBuilder config)
        {
            config.ConfigureProfiles(profile =>
            {
                profile.ClearTypeFormatters();
                profile.ClearTypeStyles();
                profile.DefaultLogValueStyle = null;
                profile.OutputTemplate = "{Message}";
            });
        }
    }
}