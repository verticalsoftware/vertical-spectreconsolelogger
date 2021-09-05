using Microsoft.Extensions.ObjectPool;
using Vertical.SpectreLogger.Output;

namespace Vertical.SpectreLogger.Internal
{
    /// <summary>
    /// Maintains reusable <see cref="IWriteBuffer"/> instances.
    /// </summary>
    internal sealed class WriteBufferPool
    {
        private const int DefaultPoolCapacity = 5;
        
        private readonly DefaultObjectPool<IWriteBuffer> _pool;

        internal WriteBufferPool(IConsoleWriter consoleWriter)
        {
            _pool = new(new WriteBufferPooledObjectPolicy(this, consoleWriter), DefaultPoolCapacity);
        }

        internal IWriteBuffer GetInstance() => _pool.Get();
        
        internal void Disposed(IWriteBuffer buffer)
        {
            buffer.Flush();
            buffer.Clear();
            
            _pool.Return(buffer);
        }
    }
}