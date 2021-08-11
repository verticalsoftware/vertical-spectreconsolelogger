using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace SpectreLoggerExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var examples = typeof(Program)
                .Assembly
                .DefinedTypes
                .Select(type => new{type, demo = type.GetCustomAttribute<DemoAttribute>()})
                .Where(item => item.demo != null)
                .ToArray();

            while (true)
            {
                AnsiConsole.MarkupLine("Select one of the following examples:");
                AnsiConsole.WriteLine();

                for (var c = 0; c < examples.Length; c++)
                {
                    AnsiConsole.MarkupLine($"[yellow]{c + 1,-2}[/] - {examples[c].demo!.Description}");
                }

                AnsiConsole.WriteLine();
                AnsiConsole.Markup("Enter the example number or type [orange1]quit[/] > ");

                var input = Console.ReadLine();

                switch (input)
                {
                    case "quit":
                        return;
                    
                    case { } when int.TryParse(input, out var id) && id > 0 && id <= examples.Length:
                        RunExample(examples[id - 1].type, examples[id -1].demo!.Description);
                        break;
                }
            }
        }

        private static void RunExample(Type type, string description)
        {
            Console.WriteLine();

            var methods = type
                .GetMethods()
                .Select(method => (method, demo: method.GetCustomAttribute<DemoAttribute>()))
                .Where(i => i.demo != null)
                .ToArray();

            var item = 1;
            var harness = Activator.CreateInstance(type);

            foreach (var (method, demo) in methods)
            {
                if (item > 1)
                {
                    AnsiConsole.WriteLine();
                    AnsiConsole.MarkupLine($"Press [{Color.Yellow.ToMarkup()}]any key[/] for the next example...");
                    Console.ReadKey();
                    AnsiConsole.WriteLine();
                }
                
                AnsiConsole.WriteLine("------------------------------------------------------------------------------");
                AnsiConsole.MarkupLine($"Example #{item}: [{Color.SeaGreen1.ToMarkup()}]{demo.Description}[/]");
                AnsiConsole.MarkupLine($"Reference source: [{Color.SeaGreen1.ToMarkup()}]{type}[/]." + 
                                       $"[{Color.Magenta1.ToMarkup()}]{method.Name}()[/]:");
                AnsiConsole.WriteLine("------------------------------------------------------------------------------");
                
                method.Invoke(harness, Array.Empty<object>());

                ++item;
            }

            Console.WriteLine();
        }
    }
}
