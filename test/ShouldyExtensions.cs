using System;
using System.Collections.Generic;
using System.Linq;
using Shouldly;

namespace Vertical.SpectreLogger.Tests
{
    public static class ShouldlyExtensions
    {
        public static void ShouldBeEqualElements<T>(this IEnumerable<T> source, IEnumerable<T> expected,
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
    }
}