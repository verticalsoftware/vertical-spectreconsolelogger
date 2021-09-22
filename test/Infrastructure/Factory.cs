using System;
using System.Collections.Generic;
using System.Linq;

namespace Vertical.SpectreLogger.Tests.Infrastructure
{
    public static class Factory
    {
        public static T New<T>(Func<T> function) => function();

        public static IEnumerable<T> New<T>(Func<T> function, int count) =>
            Enumerable.Range(0, count).Select(_ => function());
    }
}