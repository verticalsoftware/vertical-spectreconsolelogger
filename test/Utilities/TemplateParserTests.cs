using System;
using System.Collections.Generic;
using Shouldly;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Utilities;

namespace Vertical.SpectreLogger.Tests.Utilities
{
    public class TemplateParserTests
    {
        
        public void SplitReturnsExpectedSpan(string str, TemplateSpan[] expected)
        {
            TemplateParser.Split(str).ShouldBe(expected);
        }

        public static IEnumerable<object[]> SplitTheories => new[]
        {
            new object[]{ string.Empty, Array.Empty<TemplateSpan>() }
        };
    }
}