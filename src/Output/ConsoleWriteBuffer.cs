using Spectre.Console;

namespace Vertical.SpectreLogger.Output
{
    /// <summary>
    /// Default implementation of <see cref="IWriteBuffer"/> that outputs to
    /// <see cref="IAnsiConsole"/>
    /// </summary>
    internal class ConsoleWriteBuffer : WriteBuffer
    {
        private readonly IAnsiConsole _console;
        private readonly IWriteBufferFactory _factory;

        internal ConsoleWriteBuffer(IAnsiConsole console, IWriteBufferFactory factory) : base(500)
        {
            _console = console;
            _factory = factory;
        }

        public override void Dispose()
        {
            Clear();
            Margin = 0;
            _factory.WriteDisposed(this);
        }

      

        /// <inheritdoc />
        public override void Flush()
        {
            _console.Markup(ToString());
            Clear();
        }
    }
}