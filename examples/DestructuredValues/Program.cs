using System;
using Microsoft.Extensions.Logging;
using Vertical.SpectreLogger;
using Vertical.SpectreLogger.Options;

namespace DestructuredValues
{
    internal record Address(string Street, string City, string State, int ZipCode);
    internal record Person(Guid Id, string FirstName, string LastName, Address Address, string[] Roles);
    
    internal class Program
    {
        static void Main(string[] args)
        {
            var person = new Person(
                Guid.NewGuid(),
                "Testy",
                "McTesterson",
                new Address("123 Main Street", "Denver", "CO", 80101),
                new[]{"NA-Colleague", "Manager", "Sales"});

            WriteNonIndented(person);

            Console.WriteLine();
            
            WriteIndented(person);
        }

        private static void WriteNonIndented(Person person)
        {
            var logger = LoggerFactory
                .Create(builder => builder.AddSpectreConsole())
                .CreateLogger<Person>();
            
            logger.LogInformation("User created (non-indented) = {@TheUser}", person);
        }

        private static void WriteIndented(Person person)
        {
            var logger = LoggerFactory
                .Create(builder => builder.AddSpectreConsole(console => console.ConfigureProfile(
                    LogLevel.Information,
                    profile => profile.ConfigureOptions<DestructuringOptions>(ds => ds.WriteIndented = true))))
                .CreateLogger<Person>();
            
            logger.LogInformation("User created (indented) = \n{@TheUser}", person);
        }
    }
}
