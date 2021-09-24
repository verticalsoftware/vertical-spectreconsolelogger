using Microsoft.Extensions.Logging;
using Shouldly;
using Vertical.SpectreLogger.Options;
using Xunit;

namespace Vertical.SpectreLogger.Tests.Options
{
    public class SerilogStyleLoggerOptionsTests
    {
        [Fact]
        public void UseMicrosoftStyleNoThrow()
        {
            LoggerFactory.Create(builder => builder
                    .AddSpectreConsole(config => config.UseSerilogConsoleStyle()))
                .CreateLogger("Test").ShouldNotBeNull();
        }
    }
}