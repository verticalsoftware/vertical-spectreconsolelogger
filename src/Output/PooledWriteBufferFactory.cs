using Microsoft.Extensions.ObjectPool;
using Spectre.Console;

namespace Vertical.SpectreLogger.Output
{
    /// <summary>
    /// Implements a provides that pools write buffers.
    /// </summary>
    internal class PooledWriteBufferFactory : IWriteBufferFactory
    {
        private readonly DefaultObjectPool<IWriteBuffer> _writeBufferPool;
        private readonly IAnsiConsole _ansiConsole;

        private sealed class PoolPolicy : IPooledObjectPolicy<IWriteBuffer>
        {
            private readonly IAnsiConsole _ansiConsole;
            private readonly IWriteBufferFactory _factory;

            internal PoolPolicy(IAnsiConsole ansiConsole, IWriteBufferFactory factory)
            {
                _ansiConsole = ansiConsole;
                _factory = factory;
            }

            /// <inheritdoc />
            public IWriteBuffer Create() => new ConsoleWriteBuffer(_ansiConsole, _factory);

            /// <inheritdoc />
            public bool Return(IWriteBuffer obj)
            {
                obj.Clear();
                return true;
            }
        }

        internal PooledWriteBufferFactory(IAnsiConsole ansiConsole)
        {
            _writeBufferPool = new DefaultObjectPool<IWriteBuffer>(new PoolPolicy(ansiConsole, this), 16);
            _ansiConsole = ansiConsole;
        }

        /// <inheritdoc />
        public IWriteBuffer GetInstance() => new ConsoleWriteBuffer(_ansiConsole, this);

        /// <inheritdoc />
        public void WriteDisposed(IWriteBuffer writeBuffer) => _writeBufferPool.Return(writeBuffer);
    }
}