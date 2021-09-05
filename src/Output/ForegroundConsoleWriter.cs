using Spectre.Console;

namespace Vertical.SpectreLogger.Output
{
    internal class ForegroundConsoleWriter : IConsoleWriter
    {
        private readonly IAnsiConsole _console;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="console">Console instance</param>
        public ForegroundConsoleWriter(IAnsiConsole console)
        {
            _console = console;
        }
        
        /// <inheritdoc />
        public void Write(string content)
        {
            _console.Markup(content);
        }
    }
}