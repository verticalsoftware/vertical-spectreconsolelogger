using Microsoft.Extensions.ObjectPool;
using Spectre.Console;

namespace Vertical.SpectreLogger.Output
{
    /// <summary>
    /// Implements a provides that pools write buffers.
    /// </summary>
    internal class PooledWriteBufferProvider : IWriteBufferProvider
    {
        private readonly DefaultObjectPool<IWriteBuffer> _writeBufferPool;
        private readonly IAnsiConsole _ansiConsole;
        private readonly IWriteBufferProvider _provider;

        private sealed class PoolPolicy : IPooledObjectPolicy<IWriteBuffer>
        {
            private readonly IAnsiConsole _ansiConsole;
            private readonly IWriteBufferProvider _provider;

            internal PoolPolicy(IAnsiConsole ansiConsole, IWriteBufferProvider provider)
            {
                _ansiConsole = ansiConsole;
                _provider = provider;
            }

            /// <inheritdoc />
            public IWriteBuffer Create() => new DefaultWriteBuffer(_ansiConsole, _provider);

            /// <inheritdoc />
            public bool Return(IWriteBuffer obj)
            {
                obj.Clear();
                return true;
            }
        }

        internal PooledWriteBufferProvider(IAnsiConsole ansiConsole, IWriteBufferProvider provider)
        {
            _writeBufferPool = new DefaultObjectPool<IWriteBuffer>(new PoolPolicy(ansiConsole, this), 16);
            _ansiConsole = ansiConsole;
            _provider = provider;
        }

        /// <inheritdoc />
        public IWriteBuffer GetInstance() => new DefaultWriteBuffer(_ansiConsole, _provider);

        /// <inheritdoc />
        public void WriteDisposed(IWriteBuffer writeBuffer) => _writeBufferPool.Return(writeBuffer);
    }
}