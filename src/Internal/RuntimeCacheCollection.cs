using System;
using System.Collections.Concurrent;

namespace Vertical.SpectreLogger.Internal
{
    internal sealed class RuntimeCacheCollection
    {
        private readonly ConcurrentDictionary<Type, object> _cache = new();

        internal T GetOrAdd<T>(Func<T> valueFactory) where T : class => (T)_cache.GetOrAdd(typeof(T), t => valueFactory);

        /// <inheritdoc />
        public override string ToString() => _cache.Count.ToString();
    }
}