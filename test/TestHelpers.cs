using System;
using System.Collections.Generic;
using NSubstitute;
using Shouldly;
using Spectre.Console;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Output;

namespace Vertical.SpectreLogger.Tests
{
    public static class TestHelpers
    {
        public static void ShouldHaveElementsEqualTo<T>(
            this IEnumerable<T> source, 
            IEnumerable<T> expected,
            Action<T, T> comparer)
        {
            var queue = new Queue<T>(expected);

            foreach (var item in source)
            {
                queue.TryDequeue(out var nextExpected).ShouldBeTrue();

                comparer(item, nextExpected!);
            }

            queue.ShouldBeEmpty($"{queue.Count} position control(s) not matched.");
        }

        public static void VerifyOutput(
            this ITemplateRenderer renderer,
            in LogEventInfo eventInfo,
            string expectedContent)
        {
            var buffer = new WriteBuffer(Substitute.For<IAnsiConsoleWriter>());
            renderer.Render(buffer, eventInfo);
            
            buffer.ToString().ShouldBe(expectedContent);
        }
    }
}