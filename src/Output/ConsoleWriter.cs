using System;
using Spectre.Console;

namespace Vertical.SpectreLogger.Output
{
    /// <summary>
    /// Base class for console writer implementations.
    /// </summary>
    public abstract class ConsoleWriter
    {
        private readonly IAnsiConsole _console;

        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        /// <param name="console"><see cref="IAnsiConsole"/> implementation.</param>
        protected ConsoleWriter(IAnsiConsole console) => _console = console;

        /// <summary>
        /// Writes a value to the console.
        /// </summary>
        /// <param name="str">Content to write.</param>
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