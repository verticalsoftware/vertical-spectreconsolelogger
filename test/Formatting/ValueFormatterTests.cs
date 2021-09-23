using Shouldly;
using Vertical.SpectreLogger.Formatting;
using Xunit;

namespace Vertical.SpectreLogger.Tests.Formatting
{
    public class ValueFormatterTests
    {
        [Fact]
        public void FormatInvokesInternalDelegate()
        {
            const string format = "x4";
            const int input = 100;

            var output = new ValueFormatter<int>((fmt, value) =>
                {
                    fmt.ShouldBe(format);
                    value.ShouldBe(input);
                    return "true";
                })
                .Format(format, input, null);
            
            output.ShouldBe("true");
        }
    }
}