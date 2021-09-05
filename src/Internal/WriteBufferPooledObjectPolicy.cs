using Microsoft.Extensions.ObjectPool;
using Spectre.Console;
using Vertical.SpectreLogger.Output;

namespace Vertical.SpectreLogger.Internal
{
    internal class WriteBufferPooledObjectPolicy : PooledObjectPolicy<IWriteBuffer>
    {
        private readonly WriteBufferPool _bufferPool;
        private readonly IConsoleWriter _consoleWriter;

        internal WriteBufferPooledObjectPolicy(WriteBufferPool bufferPool, IConsoleWriter consoleWriter)
        {
            _bufferPool = bufferPool;
            _consoleWriter = consoleWriter;
        }

        /// <inheritdoc />
        public override IWriteBuffer Create() => new WriteBuffer(_consoleWriter, _bufferPool);

        /// <inheritdoc />
        public override bool Return(IWriteBuffer obj)
        {
            obj.Clear();
            obj.Margin = 0;

            return true;
        }
    }
}