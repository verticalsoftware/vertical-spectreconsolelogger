using Microsoft.Extensions.Logging;
using Shouldly;
using Vertical.SpectreLogger.Tests.Infrastructure;
using Xunit;

namespace Vertical.SpectreLogger.Tests.Formatting
{
    public class IntegralFormattingTests
    {
        [Fact]
        public void FormatDecimal_ReturnsExpected()
        {
            RendererTestHarness.Capture(
                cfg => cfg.ConfigureProfile(LogLevel.Information, p => p.OutputTemplate="{Message}"),
                logger => logger.LogInformation("Double: {Value:F2}", 1d))
                .ShouldBe("Double: [magenta3_2]1.00[/]");
        }
    }
}