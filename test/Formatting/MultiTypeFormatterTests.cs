using System;
using System.Collections.Generic;
using Shouldly;
using Vertical.SpectreLogger.Formatting;
using Xunit;

namespace Vertical.SpectreLogger.Tests.Formatting
{
    public class MultiTypeFormatterTests
    {
        [Fact]
        public void FormatReturnsValueForTypeUsingRegisteredFormatter()
        {
            var testInstance = new MultiTypeFormatter(new Dictionary<Type, ICustomFormatter>
            {
                [typeof(int)] = new ValueFormatter<int>((_,_) => nameof(Int32))
            });
            
            testInstance.Format(null, 100, null).ShouldBe(nameof(Int32));
        }

        [Fact]
        public void FormatReturnsValueUsingIFormattableFunction()
        {
            var testInstance = new MultiTypeFormatter(new Dictionary<Type, ICustomFormatter>());
            
            testInstance.Format("x4", 100, null).ShouldBe("0064");
        }

        [Fact]
        public void FormatReturnsDefaultToString()
        {
            var testInstance = new MultiTypeFormatter(new Dictionary<Type, ICustomFormatter>());

            testInstance.Format(null, new{value="test"}, null).ShouldBe("{ value = test }");
        }
    }
}