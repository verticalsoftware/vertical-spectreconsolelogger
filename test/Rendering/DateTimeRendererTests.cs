using System;
using Microsoft.Extensions.Logging;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Rendering;
using Vertical.SpectreLogger.Tests.Infrastructure;
using Xunit;

namespace Vertical.SpectreLogger.Tests.Rendering
{
    public class DateTimeRendererTests
    {
        [Fact]
        public void RenderWritesExpectedOutput()
        {
            var now = DateTimeOffset.Now;
            
            RendererTestHarness.RunScenario(
                opt =>
                {
                    opt.ConfigureProfiles(profile =>
                    {
                        profile.OutputTemplate = "{DateTime}";
                        // Also tests the factory
                        profile.ConfigureOptions<DateTimeRenderer.Options>(dt => dt.ValueFactory = () => now);
                    });
                },
                logger => logger.LogInformation(""),
                $@"^\[\w+\]{now}\[/\]$");
        }
    }
}