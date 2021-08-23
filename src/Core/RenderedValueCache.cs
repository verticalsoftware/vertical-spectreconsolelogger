using System;
using System.Collections.Concurrent;

namespace Vertical.SpectreLogger.Core
{
    public class RenderedValueCache
    {
        private readonly ConcurrentDictionary<(Type, object), string> _cachedValues = new();

        public bool TryGetValue<TRenderer>(object key, out string? renderedValue) where TRenderer : ITemplateRenderer
        {
            return TryGetValue(typeof(TRenderer), key, out renderedValue);
        }
        
        public bool TryGetValue(Type rendererType, object key, out string? renderedValue)
        {
            return _cachedValues.TryGetValue((rendererType, key), out renderedValue);
        }
        
        public void CacheValue<TRenderer>(object key, string renderedValue) where TRenderer : ITemplateRenderer
        {
            CacheValue(typeof(TRenderer), key, renderedValue);
        }

        public void CacheValue(Type rendererType, object key, string renderedValue)
        {
            _cachedValues.AddOrUpdate((rendererType, key), renderedValue, (k, s) => renderedValue);
        }
    }
}