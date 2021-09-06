using Microsoft.Extensions.ObjectPool;
using Spectre.Console;
using Vertical.SpectreLogger.Output;

namespace Vertical.SpectreLogger.Internal
{
    internal class WriteBufferPooledObjectPolicy : PooledObjectPolicy<IWriteBuffer>
    {
        private readonly IConsoleWriter _consoleWriter;

        internal WriteBufferPooledObjectPolicy(IConsoleWriter consoleWriter)
        {
            _consoleWriter = consoleWriter;
        }

        /// <inheritdoc />
        public override IWriteBuffer Create() => new WriteBuffer(_consoleWriter);

        /// <inheritdoc />
        public override bool Return(IWriteBuffer obj)
        {
            obj.Flush();
            obj.Margin = 0;

            return true;
        }
    }
}