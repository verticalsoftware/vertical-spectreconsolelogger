using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Vertical.SpectreLogger.Infrastructure;
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

            var pipeline = instance.CreatePipeline("{LogLevel,-8} {CategoryName:F} Hello SpectreLogger!");

            pipeline[0].ShouldBeOfType<LogLevelRenderer>();
            pipeline[1].ShouldBeOfType<StaticSpanRenderer>();
            pipeline[2].ShouldBeOfType<CategoryNameRenderer>();
            pipeline[3].ShouldBeOfType<StaticSpanRenderer>();
            pipeline[4].ShouldBeOfType<NewLineRenderer>();
        }
    }
}