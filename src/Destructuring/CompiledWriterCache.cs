using System;
using System.Collections.Concurrent;
using Vertical.SpectreLogger.Reflection;

namespace Vertical.SpectreLogger.Destructuring
{
    /// <summary>
    /// Maintains a cache of <see cref="CompiledWriter"/> delegates.
    /// </summary>
    internal static class CompiledWriterCache
    {
        private static readonly ConcurrentDictionary<Type, CompiledWriter> Cache = new();

        /// <summary>
        /// Returns a <see cref="CompiledWriter"/> for the specified type.
        /// </summary>
        /// <param name="type">Type to resolve.</param>
        /// <param name="defaultWriter">Writer to use as a default.</param>
        /// <returns><see cref="CompiledWriter"/></returns>
        internal static CompiledWriter GetInstance(Type type, CompiledWriter defaultWriter)
        {
            return Cache.GetOrAdd(type, t => CompiledWriterFactory.CreateWriter(t) ?? defaultWriter);
        }
    }
}