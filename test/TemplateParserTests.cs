using System;
using System.Collections.Generic;
using Shouldly;
using Vertical.SpectreLogger.Internal;
using Xunit;

namespace Vertical.SpectreLogger.Tests
{
    public class TemplateParserTests
    {
        [Theory, MemberData(nameof(Theories))]
        public void ParseYieldsExpected(string input, IEnumerable<(string, bool)> expected)
        {
            TemplateParser.Parse(input).ShouldBe(expected);
        }

        public static IEnumerable<object[]> Theories = new[]
        {
            new object[]{ "", Array.Empty<(string, bool)>()},
            new object[]{ "no templates", new[]{("no templates", false)}},
            new object[]{ "{name}", new[]{("{name}", true)}},
            new object[]{ "{name} is test", new[]{("{name}", true), (" is test", false)}},
            new object[]{ "{name} is {test}", new[]{("{name}", true), (" is ", false), ("{test}", true)}},
            new object[]{ "{name} is {test} with hanging", new[]{("{name}", true), (" is ", false), ("{test}", true), (" with hanging", false)}},
            new object[]{ "{one}{two}{three}", new[]{("{one}", true), ("{two}", true), ("{three}", true)}},
        };
    }
}