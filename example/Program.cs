using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Spectre.Console;

namespace SpectreLoggerExample
{
    class Program
    {
        public class Ok
        {
            /// <inheritdoc />
            public override string ToString() => "OK";
        }

        public class Failed
        {
            /// <inheritdoc />
            public override string ToString() => "Failed";
        }

        public class Person
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int Age { get; set; }
        }
        
        static void Main(string[] args)
        {
            var match = Regex.Match("Dan Ingalla 8662 E Layton Ave", "8662");
            
            AnsiConsole.MarkupLine("Well hello there");
        }

        private static Exception GetException()
        {
            try
            {
                Task.Run(() => Array.Empty<string>().First()).Wait();
            }
            catch (Exception exception)
            {
                return exception;
            }

            return new Exception();
        }
    }
}
