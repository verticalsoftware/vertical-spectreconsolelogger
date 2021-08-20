using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shouldly;
using Vertical.SpectreLogger.Infrastructure;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Rendering;
using Xunit;

namespace Vertical.SpectreLogger.Tests.Infrastructure
{
    public class TemplateRenderFactoryTests
    {
        [Fact]
        public void BuildPipelineReturnsExpectedRenderers()
        {
            var services = new ServiceCollection()
                .AddLogging(builder => builder.AddSpectreConsole())
                .BuildServiceProvider();

            var instance = services.GetRequiredService<ITemplateRendererFactory>();
            var profile = new FormattingProfile(LogLevel.Information)
            {
                OutputTemplate = "{LogLevel,-8} {CategoryName:F} Hello SpectreLogger!"
            };

            var pipeline = instance.CreatePipeline(profile);

            pipeline[0].ShouldBeOfType<LogLevelRenderer>();
            pipeline[1].ShouldBeOfType<StaticSpanRenderer>();
            pipeline[2].ShouldBeOfType<CategoryNameRenderer>();
            pipeline[3].ShouldBeOfType<StaticSpanRenderer>();
            pipeline[4].ShouldBeOfType<MarginControlRenderer>();
            pipeline[5].ShouldBeOfType<NewLineRenderer>();
        }
    }
}