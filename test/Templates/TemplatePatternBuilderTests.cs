﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Shouldly;
using Vertical.SpectreLogger.Templates;
using Xunit;

namespace Vertical.SpectreLogger.Tests.Templates
{
    public class TemplatePatternBuilderTests
    {
        [Fact]
        public void SplitPatternFieldReturnsExpectedPattern()
        {
            var pattern = TemplatePatternBuilder.SplitPattern;

            Should.NotThrow(() => new Regex(pattern));
            
            pattern.ShouldBe(@"(?<!{){(?<_tmpl>(?<_key>@?[^,:{}>]+)(?:>(?<_ctl>[^,:{}]+))?(?<_cfmt>(?<_wdspan>,(?<_wd>-?\d+))?(?<_fmspan>:(?<_fm>[^}]+))?))}");
        }

        [Theory]
        [InlineData(',')]
        [InlineData(':')]
        [InlineData('>')]
        [InlineData('{')]
        [InlineData('}')]
        public void ConstructorRejectsInvalidKeyName(char c)
        {
            Should.Throw<ArgumentException>(() => TemplatePatternBuilder.ForKey($"Has{c}"));
        }

        [Theory, MemberData(nameof(Theories))]
        public void ValueReturnsExpectedPattern(TemplatePatternBuilder patternBuilder, string expected)
        {
            var pattern = patternBuilder.Build();

            Should.NotThrow(() => new Regex(pattern));

            pattern.ShouldBe(expected);
        }

        public static IEnumerable<object[]> Theories = new[]
        {
            new object[]
            {
                TemplatePatternBuilder.ForKey("Template"),
                @"(?<!{){(?<_tmpl>(?<_key>Template))}"
            },
            new object[]
            {
                TemplatePatternBuilder.ForKey("Template")
                    .AddControlGroup(@"\d+"),
                @"(?<!{){(?<_tmpl>(?<_key>Template)(?:>(?<_ctl>\d+))?)}"
            },
            new object[]
            {
                TemplatePatternBuilder.ForKey("Template")
                    .AddControlGroup(@"\d+")
                    .AddWidthFormatting(),
                @"(?<!{){(?<_tmpl>(?<_key>Template)(?:>(?<_ctl>\d+))?(?<_cfmt>(?<_wdspan>,(?<_wd>-?\d+))?))}"
            },
            new object[]
            {
                TemplatePatternBuilder.ForKey("Template")
                    .AddControlGroup(@"\d+")
                    .AddWidthFormatting()
                    .AddValueFormatting(),
                @"(?<!{){(?<_tmpl>(?<_key>Template)(?:>(?<_ctl>\d+))?(?<_cfmt>(?<_wdspan>,(?<_wd>-?\d+))?(?<_fmspan>:(?<_fm>[^}]+))?))}"
            }
        };
    }
}