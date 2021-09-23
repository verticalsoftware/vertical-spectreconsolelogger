using Microsoft.Extensions.Logging;
using Shouldly;
using Vertical.SpectreLogger.Tests.Infrastructure;
using Xunit;

namespace Vertical.SpectreLogger.Tests.Rendering
{
    public class StaticSpanRendererTests
    {
        [Fact]
        public void RenderWritesExpectedOutput()
        {
            RendererTestHarness.Capture(
                config => config.ConfigureProfile(LogLevel.Information, profile => profile.OutputTemplate = "{Message}+static"),
                logger => logger.LogInformation("test"))
                .ShouldBe("test+static");
        }
    }
}