using System;
using NSubstitute;
using Shouldly;
using Vertical.SpectreLogger.Formatting;
using Xunit;

namespace Vertical.SpectreLogger.Tests.Formatting
{
    public class ProviderFormatterTests
    {
        [Fact]
        public void FormatInvokesInternalDelegateWithProvider()
        {
            const string format = "x4";
            const int input = 100;
            
            var providerSub = Substitute.For<IFormatProvider>();

            var output = new ProviderFormatter<int>((fmt, value, provider) =>
                {
                    fmt.ShouldBe(format);
                    providerSub.ShouldBe(provider);
                    value.ShouldBe(input);
                    return "true";
                })
                .Format(format, input, providerSub);
            
            output.ShouldBe("true");
        }
    }
}