using System;
using System.Collections.Generic;
using Shouldly;
using Vertical.SpectreLogger.Formatting;
using Vertical.SpectreLogger.Rendering;
using Xunit;

namespace Vertical.SpectreLogger.Tests.Rendering
{
    public class CategoryNameRendererTests
    {
        private readonly ICustomFormatter _testInstance = new CategoryNameRenderer.DefaultFormatter();
        
        [Theory, MemberData(nameof(Theories))]
        public void FormatReturnsExpected(string? format, string arg, string expected)
        {
            _testInstance.Format(format, new CategoryName(arg), null).ShouldBe(expected);
        }

        public static IEnumerable<object?[]> Theories => new[]
        {
            new object?[]{ null, "Logger", "Logger" },
            new object?[]{ null, "", "" },
            new object?[]{ "C", "Logger", "Logger" },
            new object?[]{ "C", "System.Logger", "Logger" },
            new object?[]{ "C1", "System.Logger", "Logger" },
            new object?[]{ "C2", "System.Logger", "System.Logger" },
            new object?[]{ "C2", "System.Logging.Logger", "Logging.Logger" }
        };
    }
}