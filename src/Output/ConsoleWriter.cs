using System;
using Spectre.Console;

namespace Vertical.SpectreLogger.Output
{
    public abstract class ConsoleWriter
    {
        private readonly IAnsiConsole _console;

        protected ConsoleWriter(IAnsiConsole console) => _console = console;

        protected void WriteToConsole(string str)
        {
            try
            {
                _console.Markup(str);
            }
            catch (Exception exception)
            {
                _console.WriteLine("Markup error: " + exception.Message);
                _console.WriteLine("Tried to write:");
                _console.WriteLine(str);
            }
        }
    }
}