using Spectre.Console;

namespace Vertical.SpectreLogger.Output
{
    internal class DefaultWriteBufferFactory : IWriteBufferFactory
    {
        private readonly IAnsiConsole _ansiConsole;
        
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="ansiConsole">Ansi console</param>
        public DefaultWriteBufferFactory(IAnsiConsole ansiConsole)
        {
            _ansiConsole = ansiConsole;
        }
        
        /// <inheritdoc />
        public IWriteBuffer GetInstance()
        {
        }

        internal void BufferDisposed(IWriteBuffer instance)
        {
        }
    }
}