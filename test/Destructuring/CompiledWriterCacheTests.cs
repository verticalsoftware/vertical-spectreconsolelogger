using Shouldly;
using Vertical.SpectreLogger.Destructuring;
using Xunit;

namespace Vertical.SpectreLogger.Tests.Destructuring
{
    public class CompiledWriterCacheTests
    {
        [Fact]
        public void CacheReturnsExpectedRef()
        {
            var type = new {type = "TestClass"}.GetType();

            var writer = CompiledWriterCache.GetInstance(type, (_, _) => { });

            writer.ShouldNotBeNull();
            CompiledWriterCache.GetInstance(type, (_, _) => {}).ShouldBe(writer);
        }
    }
}