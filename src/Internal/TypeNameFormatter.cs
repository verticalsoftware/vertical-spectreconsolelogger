using System;
using System.Collections.Concurrent;
using System.Text;
using Microsoft.Extensions.ObjectPool;

namespace Vertical.SpectreLogger.Internal
{
    internal static class TypeNameFormatter
    {
        private static readonly ObjectPool<StringBuilder> StringBuilderPool = new DefaultObjectPool<StringBuilder>(
            new StringBuilderPooledObjectPolicy(), 16);

        private static readonly ConcurrentDictionary<Type, string> Cached = new();

        internal static string Format(Type type)
        {
            return Cached.GetOrAdd(type, Build);
        }

        private static string Build(Type type)
        {
            var sb = StringBuilderPool.Get();

            try
            {
                Build(type, sb);
                return sb.ToString();
            }
            finally
            {
                StringBuilderPool.Return(sb);
            }
        }

        private static void Build(Type type, StringBuilder sb)
        {
            if (!string.IsNullOrWhiteSpace(type.Namespace))
            {
                sb.Append(type.Namespace);
                sb.Append('.');
            }

            sb.Append(type.Name);
        }
    }
}