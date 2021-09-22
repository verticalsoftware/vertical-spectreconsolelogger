using System.Linq;
using Microsoft.Extensions.ObjectPool;
using NSubstitute;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.Rendering;
using Vertical.SpectreLogger.Templates;
using Xunit;

namespace Vertical.SpectreLogger.Tests.Rendering
{
    public class RendererPipelineTests
    {
        [Fact]
        public void RenderInvokesPipeline()
        {
            var options = new SpectreLoggerOptions();
            var rendererBuilder = Substitute.For<ITemplateRendererBuilder>();
            var pool = Substitute.For<ObjectPool<IWriteBuffer>>();
            pool.Get().Returns(Substitute.For<IWriteBuffer>());
            var renderers = Enumerable.Range(0, 5).Select(_ =>
            {
                var renderer = Substitute.For<ITemplateRenderer>();
                return renderer;
            }).ToArray();
            rendererBuilder.GetRenderers(Arg.Any<string>()).Returns(renderers);
            
            var testInstance = new RendererPipeline()
        }
    }
}