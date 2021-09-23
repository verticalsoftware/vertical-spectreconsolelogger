using System.Threading;
using Microsoft.Extensions.Logging;
using Shouldly;
using Vertical.SpectreLogger.Tests.Infrastructure;
using Xunit;

namespace Vertical.SpectreLogger.Tests.Rendering
{
    public class ThreadIdRendererTests
    {
        [Fact]
        public void RenderWritesExpectedOutput()
        {
            RendererTestHarness.Capture(config => config.ConfigureProfile(LogLevel.Information,
                profile =>
                {
                    profile.DefaultLogValueStyle = null;
                    profile.OutputTemplate = "{ThreadId:x4}";
                }),
                logger => logger.LogInformation(""))
                .ShouldBe(Thread.CurrentThread.ManagedThreadId.ToString("x4"));
        }
    }
}