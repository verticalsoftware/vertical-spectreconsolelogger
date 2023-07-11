using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Vertical.SpectreLogger.Output;
using Xunit;

namespace Vertical.SpectreLogger.Tests.Threading
{
    public class MultiThreadedLoggingTests : IClassFixture<MultiThreadedLoggingTests.Fixture>
    {
        private readonly Fixture _fixture;
        
        public MultiThreadedLoggingTests(Fixture fixture) => _fixture = fixture;
        public class Fixture
        {
            private readonly IServiceProvider _provider;
            
            public Fixture()
            {
                var services = new ServiceCollection();
                services.AddLogging(builder => builder.AddSpectreConsole());
                services.Replace(ServiceDescriptor.Singleton(
                    Substitute.For<IConsoleWriter>()));
                _provider = services.BuildServiceProvider();
            }

            public ILogger GetLogger() => _provider.GetRequiredService<ILoggerFactory>()
                .CreateLogger("test");
        }

        [Fact]
        public async Task LogFromMultipleThreadsDoesNotThrow()
        {
            var logger = _fixture.GetLogger();
            var threads = Enumerable
                .Range(0, 25)
                .Select(id => Task.Run(async () =>
                {
                    await Task.Delay(250);
                    for (var c = 0; c < 25; c++)
                    {
                        if (c % 10 == 0)
                        {
                            var exception = new NotSupportedException();
                            logger.LogError(exception, "An error is being reported, iteration {i}, thread {t}",
                                c, id);
                            continue;
                        }

                        logger.LogInformation("Information is being reported, iteration {i}, thread{t}",
                            c, id);
                    }
                }));
            await Task.WhenAll(threads);
        }
    }
}