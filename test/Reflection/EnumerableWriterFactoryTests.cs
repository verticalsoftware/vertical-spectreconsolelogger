using NSubstitute;
using Shouldly;
using Vertical.SpectreLogger.Destructuring;
using Vertical.SpectreLogger.Reflection;
using Xunit;

namespace Vertical.SpectreLogger.Tests.Reflection
{
    public class EnumerableWriterFactoryTests
    {
        [Fact]
        public void TryCreateReturnsCompiledWriter()
        {
            var obj = new[] {"one", "two", "three"};

            EnumerableWriterFactory.TryCreate(obj.GetType(), out var function).ShouldBeTrue();

            var writer = Substitute.For<IDestructuringWriter>();
            writer.WriteElement(Arg.Any<object>()).Returns(true);

            function!(writer, obj);
            
            writer.Received().WriteStartArray();
            writer.Received().WriteElement((object)"one");
            writer.Received().WriteElement((object)"two");
            writer.Received().WriteElement((object)"three");
            writer.Received().WriteEndArray();
        }
    }
}