using System.Collections.Generic;
using NSubstitute;
using Shouldly;
using Vertical.SpectreLogger.Destructuring;
using Vertical.SpectreLogger.Reflection;
using Xunit;

namespace Vertical.SpectreLogger.Tests.Reflection
{
    public class DictionaryWriterFactoryTests
    {
        [Fact]
        public void TryCreateReturnsCompiledInstance()
        {
            var dictionary = new Dictionary<int, string>()
            {
                [1] = "one",
                [2] = "two",
                [3] = "three"
            };
            
            DictionaryWriterFactory.TryCreate(dictionary.GetType(), out var function)
                .ShouldBeTrue();

            var writer = Substitute.For<IDestructuringWriter>();
            writer.WriteProperty(Arg.Any<string>(), Arg.Any<object>()).Returns(true);
            
            function.ShouldNotBeNull();
            function(writer, dictionary);

            writer.Received().WriteStartObject();
            writer.Received().WriteProperty("1", (object)"one");
            writer.Received().WriteProperty("2", (object)"two");
            writer.Received().WriteProperty("3", (object)"three");
            writer.Received().WriteEndObject();
        }
    }
}