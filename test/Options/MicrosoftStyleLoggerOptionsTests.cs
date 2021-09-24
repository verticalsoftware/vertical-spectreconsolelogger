using Microsoft.Extensions.Logging;
using Shouldly;
using Vertical.SpectreLogger.Options;
using Xunit;

namespace Vertical.SpectreLogger.Tests.Options
{
    public class MicrosoftStyleLoggerOptionsTests
    {
        [Fact]
        public void UseMicrosoftStyleNoThrow()
        {
            LoggerFactory.Create(builder => builder
                    .AddSpectreConsole(config => config.UseMicrosoftConsoleStyle()))
                .CreateLogger("Test").ShouldNotBeNull();
        }
    }
}