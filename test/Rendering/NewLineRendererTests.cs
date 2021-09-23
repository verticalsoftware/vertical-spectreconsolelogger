using System;
using Microsoft.Extensions.Logging;
using Shouldly;
using Vertical.SpectreLogger.Tests.Infrastructure;
using Xunit;

namespace Vertical.SpectreLogger.Tests.Rendering
{
    public class NewLineRendererTests
    {
        [Fact]
        public void RenderWritesExpectedOutput()
        {
            RendererTestHarness.Capture(config => config.ConfigureProfile(LogLevel.Information,
                profile => profile.OutputTemplate = "Line1{NewLine}Line2"),
                logger => logger.LogInformation(""))
                .ShouldBe($"Line1{Environment.NewLine}Line2");
        }
    }
}