using Microsoft.Extensions.ObjectPool;
using Spectre.Console;
using Vertical.SpectreLogger.Output;

namespace Vertical.SpectreLogger.Internal
{
    internal class WriteBufferPooledObjectPolicy : PooledObjectPolicy<IWriteBuffer>
    {
        private readonly IAnsiConsoleWriter _consoleWriter;

        internal WriteBufferPooledObjectPolicy(IAnsiConsoleWriter consoleWriter)
        {
            _consoleWriter = consoleWriter;
        }

        /// <inheritdoc />
        public override IWriteBuffer Create() => new WriteBuffer(_consoleWriter);

        /// <inheritdoc />
        public override bool Return(IWriteBuffer obj)
        {
            obj.Clear();
            return true;
        }
    }
}