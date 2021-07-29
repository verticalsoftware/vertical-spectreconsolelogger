using Spectre.Console;
using Vertical.SpectreLogger.Memory;

namespace Vertical.SpectreLogger.Output
{
    internal class DefaultWriteBufferFactory : IWriteBufferFactory
    {
        private readonly IStringBuilderPool _stringBuilderPool;
        private readonly IAnsiConsole _ansiConsole;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="stringBuilderPool">String builder pool.</param>
        /// <param name="ansiConsole">Ansi console</param>
        public DefaultWriteBufferFactory(IStringBuilderPool stringBuilderPool, IAnsiConsole ansiConsole)
        {
            _stringBuilderPool = stringBuilderPool;
            _ansiConsole = ansiConsole;
        }
        
        /// <inheritdoc />
        public IWriteBuffer GetInstance()
        {
            return new AnsiConsoleBuffer(_stringBuilderPool, _ansiConsole);
        }
    }
}