using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;
using Vertical.SpectreLogger.Destructuring;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Output;
using Xunit;

namespace Vertical.SpectreLogger.Tests.Destructuring
{
    public class DestructuringWriterTests
    {
        [Fact]
        public void WriteDictionaryRendersExpectedContent()
        {
            var profile = new LogLevelProfile(LogLevel.Information);
            var buffer = new WriteBuffer(Substitute.For<IConsoleWriter>());
            var dictionary = new Dictionary<string, object>
            {
                ["firstName"] = "Testy",
                ["lastName"] = "McTesterson",
                ["address"] = new Dictionary<string, string>
                {
                    ["street"] = "123 Main Street",
                    ["city"] = "Denver",
                    ["state"] = "CO"
                }
            };
            
            DestructuringWriter.Write(buffer, profile, dictionary);
            
            buffer.ToString().ShouldBe("{firstName: Testy, lastName: McTesterson, address: {street: 123 Main Street, city: Denver, state: CO}}");
        }

        [Fact]
        public void WriteObjectRendersExpectedContent()
        {
            var obj = new
            {
                FirstName = "Testy",
                LastName = "McTesterson",
                Address = new
                {
                    Street = "123 Main Street",
                    City = "Denver",
                    State = "CO"
                }
            };

            var buffer = new WriteBuffer(Substitute.For<IConsoleWriter>());
            var profile = new LogLevelProfile(LogLevel.Information);
            
            DestructuringWriter.Write(buffer, profile, obj);
            
            buffer.ToString().ShouldBe("{FirstName: Testy, LastName: McTesterson, Address: {Street: 123 Main Street, City: Denver, State: CO}}");
        }

        [Fact]
        public void WriteArrayRendersExpectedContent()
        {
            var obj = new[] {"one", "two", "three"};

            var buffer = new WriteBuffer(Substitute.For<IConsoleWriter>());
            var profile = new LogLevelProfile(LogLevel.Information);
            
            DestructuringWriter.Write(buffer, profile, obj);
            
            buffer.ToString().ShouldBe("[[one, two, three]]");
        }

        record Person(string Name, Person[]? Children)
        {
            /// <inheritdoc />
            public override string ToString() => $"{Name}, Children={Children?.Length}";
        }

        [Fact]
        public void WriteDictionaryRespectsMaxDepth()
        {
            var obj = new Person(
                "Testy",
                new[]
                {
                    new Person(
                        "Testy Jr.",
                        new[]
                        {
                            new Person(
                                "Testy III",
                                new[]
                                {
                                    new Person("Testy IV",
                                        Array.Empty<Person>())
                                })
                        })
                });

            var profile = new LogLevelProfile(LogLevel.Information);
            profile.ConfigureRenderer<DestructuringOptions>(opt => opt.MaxDepth = 3);

            var buffer = new WriteBuffer(Substitute.For<IConsoleWriter>());
            DestructuringWriter.Write(buffer, profile, obj);
        }
    }
}