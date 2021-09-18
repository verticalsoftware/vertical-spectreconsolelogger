using System;
using Microsoft.Extensions.Logging;
using Vertical.SpectreLogger;

namespace DestructuredValues
{
    internal record Address(string Street, string City, string State, int ZipCode);
    internal record Person(Guid Id, string FirstName, string LastName, Address Address);
    
    internal class Program
    {
        static void Main(string[] args)
        {
            var person = new Person(
                Guid.NewGuid(),
                "Testy",
                "McTesterson",
                new Address("123 Main Street", "Denver", "CO", 80101));

            var logger = LoggerFactory
                .Create(builder => builder.AddSpectreConsole())
                .CreateLogger<Person>();
            
            logger.LogInformation("User created = {@TheUser}", person);
        }
    }
}
