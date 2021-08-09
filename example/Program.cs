using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Vertical.SpectreLogger;

namespace SpectreLoggerExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var examples = typeof(Program)
                .Assembly
                .DefinedTypes
                .Where(type => typeof(IExample).IsAssignableFrom(type) && type.IsClass)
                .Select(Activator.CreateInstance)
                .Cast<IExample>()
                .OrderBy(example => example.Description)
                .ToArray();

            while (true)
            {

                AnsiConsole.MarkupLine("Select one of the following examples:");
                AnsiConsole.WriteLine();

                for (var c = 0; c < examples.Length; c++)
                {
                    AnsiConsole.MarkupLine($"[yellow]{c + 1,-2}[/] - {examples[c].Description}");
                }

                AnsiConsole.WriteLine();
                AnsiConsole.Markup("Enter the example number or type [orange1]quit[/] > ");

                var input = Console.ReadLine();

                switch (input)
                {
                    case "quit":
                        return;
                    
                    case { } when int.TryParse(input, out var id) && id > 0 && id <= examples.Length:
                        RunExample(examples[id - 1]);
                        break;
                }
            }
        }

        private static void RunExample(IExample example)
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.ClearProviders();
                builder.SetMinimumLevel(LogLevel.Trace);
                builder.AddSpectreConsole(example.Configuration);
            });

            var logger = loggerFactory.CreateLogger("Vertical.SpectreLogger.Example");

            Console.WriteLine();
            
            example.Demo(logger);

            Console.WriteLine();
        }
    }
}
