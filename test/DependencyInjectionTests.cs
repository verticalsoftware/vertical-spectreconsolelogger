using Microsoft.Extensions.Logging;
using Shouldly;
using Xunit;

namespace Vertical.SpectreLogger.Tests
{
    public class DependencyInjectionTests
    {
        [Fact]
        public void ServicesAndDependenciesCreated()
        {
            var factory = LoggerFactory.Create(builder => builder.AddSpectreConsole());

            var logger = factory.CreateLogger<DependencyInjectionTests>();
            
            logger.ShouldNotBeNull();
        }
    }
}