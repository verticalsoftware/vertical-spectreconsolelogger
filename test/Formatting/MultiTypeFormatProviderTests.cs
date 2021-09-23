using System;
using NSubstitute;
using Shouldly;
using Vertical.SpectreLogger.Formatting;
using Xunit;

namespace Vertical.SpectreLogger.Tests.Formatting
{
    public class MultiTypeFormatProviderTests
    {
        [Fact]
        public void GetFormatReturnsInstance()
        {
            var testInstance = new MultiTypeFormatProvider(Substitute.For<ICustomFormatter>());

            testInstance.GetFormat(typeof(ICustomFormatter))
                .ShouldNotBeNull()
                .ShouldBeAssignableTo<ICustomFormatter>();
        }
    }
}