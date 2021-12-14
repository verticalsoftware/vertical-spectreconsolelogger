using System;
using System.Collections.Generic;
using Shouldly;
using Vertical.SpectreLogger.Internal;
using Xunit;

namespace Vertical.SpectreLogger.Tests.Infrastructure
{
    public class StackFrameInfoTests
    {
        [Theory, MemberData(nameof(Theories))]
        public void TryParseReturnsExpectedMatches(
            string frame,
            string? expectedMethod,
            string? expectedFile,
            int? expectedLineNumber,
            (string type, string name)[]? expectedParameters)
        {
            StackFrameInfo.TryParse(frame, out var testInstance).ShouldBe(true);

            testInstance.Matched.ShouldBe(true);
            testInstance.File.ShouldBe(expectedFile);
            testInstance.Method.ShouldBe(expectedMethod);
            testInstance.LineNumber.ShouldBe(expectedLineNumber);
            testInstance.Parameters.ShouldBe(expectedParameters);
        }

        private static readonly (string type, string name)[] NoParams = Array.Empty<(string, string)>();
        
        public static IEnumerable<object?> Theories => new[]
        {
            new object?[]
            {
                "   at System.Threading.Tasks.Task.Wait()",
                "System.Threading.Tasks.Task.Wait",
                null,
                null,
                NoParams
            },
            new object?[]
            {
                "   at System.Linq.Enumerable.First[TSource]()",
                "System.Linq.Enumerable.First[TSource]",
                null,
                null,
                NoParams
            },
            new object?[]
            {
                "   at System.Threading.Tasks.Task.<>c.<.cctor>b__271_0()",
                "System.Threading.Tasks.Task.<>c.<.cctor>b__271_0",
                null,
                null,
                NoParams
            },
            new object?[]
            {
                "   at System.Threading.Tasks.Task.Wait(int a)",
                "System.Threading.Tasks.Task.Wait",
                null,
                null,
                new[]
                {
                    ("int", "a")
                }
            },
            new object?[]
            {
                "   at System.Threading.Tasks.Task.Wait(System.Int32 a)",
                "System.Threading.Tasks.Task.Wait",
                null,
                null,
                new[]
                {
                    ("System.Int32", "a")
                }
            },
            new object?[]
            {
                "   at System.Threading.Tasks.Task.Wait(Int32& a)",
                "System.Threading.Tasks.Task.Wait",
                null,
                null,
                new[]
                {
                    ("Int32&", "a")
                }
            },
            new object?[]
            {
                "   at System.Threading.Tasks.Task.Wait(Int32* a)",
                "System.Threading.Tasks.Task.Wait",
                null,
                null,
                new[]
                {
                    ("Int32*", "a")
                }
            },
            new object?[]
            {
                "   at System.Threading.Tasks.Task.Wait(List`1 a)",
                "System.Threading.Tasks.Task.Wait",
                null,
                null,
                new[]
                {
                    ("List`1", "a")
                }
            },
            new object?[]
            {
                "   at System.Threading.Tasks.Task.Wait(int a, bool b, List`1 c)",
                "System.Threading.Tasks.Task.Wait",
                null,
                null,
                new[]
                {
                    ("int", "a"),
                    ("bool", "b"),
                    ("List`1", "c")
                }
            },
            new object?[]
            {
                @"   at System.Threading.Tasks.Task.Wait() in c:\Users\vertical\src\SourceFile.cs:line 1",
                "System.Threading.Tasks.Task.Wait",
                @"c:\Users\vertical\src\SourceFile.cs",
                1,
                NoParams
            },
            new object?[]
            {
                @"   at System.Threading.Tasks.Task.Wait() in /usr/vertical/.share/source/SourceFile.cs:line 1",
                "System.Threading.Tasks.Task.Wait",
                @"/usr/vertical/.share/source/SourceFile.cs",
                1,
                NoParams
            },
            new object?[]
            {
                @"   at System.Threading.Tasks.Task.Wait() in /usr/vertical/.share/source/SourceFile.cs:line 1988",
                "System.Threading.Tasks.Task.Wait",
                @"/usr/vertical/.share/source/SourceFile.cs",
                1988,
                NoParams
            },
        };
    }
}