using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vertical.SpectreLogger.Memory
{
    /// <summary>
    /// Maintains a pool of string builders.
    /// </summary>
    public class StringBuilderPool : IStringBuilderPool
    {
        private readonly Queue<StringBuilder> _pool;
        private readonly int _capacity;
        private readonly object _guard = new();

        private const int StringBuilderInitialCapacity = 1000;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="capacity">The maximum number of string builders to pool.</param>
        internal StringBuilderPool(int capacity)
        {
            _capacity = capacity;
            _pool = new Queue<StringBuilder>(Enumerable.Range(0, capacity).Select(_ => 
                new StringBuilder(StringBuilderInitialCapacity)));
        }

        public StringBuilder Get()
        {
            lock (_guard)
            {
                return _pool.Any() ? _pool.Dequeue() : new StringBuilder(StringBuilderInitialCapacity);
            }
        }

        public void Return(StringBuilder builder)
        {
            builder.Clear();

            lock (_guard)
            {
                if (_pool.Count < _capacity)
                {
                    _pool.Enqueue(builder);
                }
            }
        }
    }
}