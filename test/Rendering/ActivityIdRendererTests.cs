using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;
using Spectre.Console;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.Rendering;
using Vertical.SpectreLogger.Templates;
using Vertical.SpectreLogger.Tests.Infrastructure;
using Xunit;

namespace Vertical.SpectreLogger.Tests.Rendering
{
    public class ActivityIdRendererTests
    {
        [Fact]
        public void RendererWritesExpectedValue()
        {
            var activity = new Activity("test");
            activity.Start();

            try
            {
                RendererTestHarness.RunScenario(
                    config => config.ConfigureProfiles(p => p.OutputTemplate = "{ActivityId}"),
                    logger => logger.LogInformation(""),
                    @$"^\[\w+\]{activity.TraceId}\[/\]$");
            }
            finally
            {
                activity.Stop();
            }
        }

        [Fact]
        public void RendererWritesNothingWithNoActivityId()
        {
            RendererTestHarness.RunScenario(
                config => config.ConfigureProfiles(p => p.OutputTemplate = "{ActivityId}"),
                logger => logger.LogInformation(""),
                "");
        }
    }
}