using System;
using Microsoft.Extensions.ObjectPool;
using Spectre.Console;
using Vertical.SpectreLogger.Output;

namespace Vertical.SpectreLogger.Internal
{
    internal class WriteBufferPooledObjectPolicy : PooledObjectPolicy<IWriteBuffer>
    {
        private readonly IConsoleWriter _consoleWriter;
        private readonly Func<IWriteBuffer> _writeBufferFactory;

        internal WriteBufferPooledObjectPolicy(IConsoleWriter consoleWriter, Func<IWriteBuffer> writeBufferFactory)
        {
            _consoleWriter = consoleWriter;
            _writeBufferFactory = writeBufferFactory;
        }

        /// <inheritdoc />
        public override IWriteBuffer Create() => _writeBufferFactory();

        /// <inheritdoc />
        public override bool Return(IWriteBuffer obj)
        {
            obj.Flush();
            obj.Margin = 0;

            return true;
        }
    }
}