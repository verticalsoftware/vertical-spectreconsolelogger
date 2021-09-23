using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Shouldly;
using Vertical.SpectreLogger.Tests.Infrastructure;
using Xunit;

namespace Vertical.SpectreLogger.Tests.Rendering
{
    public class ProcessIdRendererTests
    {
        [Fact]
        public void RenderWritesExpectedOutput()
        {
            RendererTestHarness.Capture(config => config.ConfigureProfile(LogLevel.Information,
                profile =>
                {
                    profile.DefaultLogValueStyle = null;
                    profile.OutputTemplate = "{ProcessId:x4}";
                }),
                logger => logger.LogInformation(""))
                .ShouldBe(Process.GetCurrentProcess().Id.ToString("x4"));
        }
    }
}