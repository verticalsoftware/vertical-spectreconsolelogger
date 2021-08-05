using System;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using Spectre.Console;
using Vertical.SpectreLogger.Options;

namespace Vertical.SpectreLogger.Output
{
    internal class DefaultWriteBufferFactory : IWriteBufferFactory
    {
        private readonly DefaultObjectPool<IWriteBuffer> _bufferPool;

        private sealed class PoolPolicy : PooledObjectPolicy<IWriteBuffer>
        {
            private readonly Func<IWriteBuffer> _factory;

            internal PoolPolicy(Func<IWriteBuffer> factory)
            {
                _factory = factory;
            }

            /// <inheritdoc />
            public override IWriteBuffer Create() => _factory();

            /// <inheritdoc />
            public override bool Return(IWriteBuffer obj) => true;
        }
        
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="optionsProvider">Options provider.</param>
        /// <param name="ansiConsole">Ansi console</param>
        public DefaultWriteBufferFactory(IOptions<SpectreLoggerOptions> optionsProvider, IAnsiConsole ansiConsole)
        {
            _bufferPool = new DefaultObjectPool<IWriteBuffer>(new PoolPolicy(() => new AnsiConsoleBuffer(this, ansiConsole)), 
                optionsProvider.Value.MaxPooledWriteBuffers);
        }

        /// <inheritdoc />
        public IWriteBuffer GetInstance() => _bufferPool.Get();

        internal void BufferDisposed(IWriteBuffer instance) => _bufferPool.Return(instance);
    }
}