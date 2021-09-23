using Shouldly;
using Vertical.SpectreLogger.Formatting;
using Xunit;

namespace Vertical.SpectreLogger.Tests.Formatting
{
    public class NullValueTests
    {
        [Fact]
        public void FormatReturnsExpectedConstant()
        {
            NullValue.Default.ToString(null, null).ShouldBe("(null)");
        }
    }
}