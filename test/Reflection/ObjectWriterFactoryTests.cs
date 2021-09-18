using NSubstitute;
using Shouldly;
using Vertical.SpectreLogger.Destructuring;
using Vertical.SpectreLogger.Reflection;
using Xunit;

namespace Vertical.SpectreLogger.Tests.Reflection
{
    public class ObjectWriterFactoryTests
    {
        [Fact]
        public void TryCreateReturnsWriter()
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

            ObjectWriterFactory.TryCreate(obj.GetType(), out var writer).ShouldBeTrue();

            var destructingWriter = Substitute.For<IDestructuringWriter>();

            writer!(destructingWriter, obj);
            
            destructingWriter.Received(1).WriteStartObject();
            destructingWriter.Received().WriteProperty("FirstName", (object)"Testy");
            destructingWriter.Received().WriteProperty("LastName", (object)"McTesterson");
            destructingWriter.Received().WriteProperty("Address", Arg.Any<object>());
            destructingWriter.Received(1).WriteEndObject();
        }
    }
}