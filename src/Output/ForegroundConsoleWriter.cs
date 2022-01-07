using Spectre.Console;

namespace Vertical.SpectreLogger.Output
{
    internal class ForegroundConsoleWriter : ConsoleWriter, IConsoleWriter
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="console">Console instance</param>
        public ForegroundConsoleWriter(IAnsiConsole console) : base(console)
        {
        }

        /// <inheritdoc />
        public void ResetLine() => ResetLineCore();

        /// <inheritdoc />
        public void Write(string content) => WriteToConsole(content);
    }
}