using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Shouldly;
using Vertical.SpectreLogger.Tests.Infrastructure;
using Xunit;

namespace Vertical.SpectreLogger.Tests.Rendering
{
    public class EndEventRendererTests
    {
        [Theory, MemberData(nameof(Theories))]
        public void RenderWritesExpectedOutput(string input, string expected)
        {
            RendererTestHarness.RunScenario(config => config.ConfigureProfiles(
                profile => profile.OutputTemplate="{Message}"),
                // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
                logger => logger.LogInformation(input),
                output => output.ShouldBe(expected));
        }
        
        public static IEnumerable<object[]> Theories => new[]
        {
            new object[]{"test", "test" + Environment.NewLine},
            new object[]{"", ""},
            new object[]{Environment.NewLine, Environment.NewLine}
        };
    }
}