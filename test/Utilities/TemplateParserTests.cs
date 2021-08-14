using System;
using System.Collections.Generic;
using System.Linq;
using Shouldly;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Utilities;
using Xunit;

namespace Vertical.SpectreLogger.Tests.Utilities
{
    public class TemplateParserTests
    {
        [Theory, MemberData(nameof(SplitTheories))]
        public void SplitReturnsExpectedSpan(string str, TemplateSpan[] expected)
        {
            var queue = new Queue<TemplateSpan>(expected);
            TemplateParser.Split(str).All(span =>
            {
                var compare = queue.Dequeue();
                span.Source.ShouldBe(compare.Source);
                span.StartIndex.ShouldBe(compare.StartIndex);
                span.Length.ShouldBe(compare.Length);
                span.Value.ShouldBe(compare.Value);
                return true;
            }).ShouldBeTrue();
        }

        public static IEnumerable<object[]> SplitTheories => new[]
        {
            new object[] {string.Empty, Array.Empty<TemplateSpan>()},
            new object[] {"span", new[] {new TemplateSpan("span", 0, 4)}},
            new object[] {"{template}", new[]{new TemplateSpan("{template}", 0, 10)}},
            new object[] {"span {template}", new[]
            {
                new TemplateSpan("span {template}", 0, 5),
                new TemplateSpan("span {template}", 5, 10)
            }},
            new object[] {"span {template} span", new[]
            {
                new TemplateSpan("span {template} span", 0, 5),
                new TemplateSpan("span {template} span", 5, 10),
                new TemplateSpan("span {template} span", 15, 5)
            }}
        };
    }
}