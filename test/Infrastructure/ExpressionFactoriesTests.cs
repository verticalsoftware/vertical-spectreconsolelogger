using Shouldly;
using Vertical.SpectreLogger.Infrastructure;
using Vertical.SpectreLogger.Types;
using Xunit;

namespace Vertical.SpectreLogger.Tests.Infrastructure
{
    public class ExpressionFactoriesTests
    {
        [Fact]
        public void CreateDestructuredObjectFactoryReturnsFunction()
        {
            var factory = ExpressionFactories.CreateDestructedObjectFactory(typeof(int));
            var instance = factory(100);

            (instance as DestructuredValue<int>)!.Value.ShouldBe(100);
        }
    }
}