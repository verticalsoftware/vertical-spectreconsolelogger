using System;
using System.Collections.Generic;
using System.Linq;
using Vertical.SpectreLogger.Core;

namespace Vertical.SpectreLogger.Options
{
    /// <summary>
    /// Manages options for renderer types.
    /// </summary>
    public class RendererOptionsCollection
    {
        private readonly Dictionary<Type, IRendererOptions> _options = new();

        internal RendererOptionsCollection()
        {
        }
        
        /// <summary>
        /// Configures an options object for a renderer type.
        /// </summary>
        /// <param name="configure">Delegate that configures the provided object.</param>
        /// <typeparam name="TOptions">Options type.</typeparam>
        /// <exception cref="ArgumentNullException"><paramref name="configure"/> delegate is null</exception>
        public void Configure<TOptions>(Action<TOptions> configure) where TOptions : IRendererOptions, new()
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }
            
            configure(GetValue<TOptions>());
        }

        /// <summary>
        /// Retrieves the value of an options object.
        /// </summary>
        /// <typeparam name="TOptions">Options type.</typeparam>
        /// <returns></returns>
        public TOptions GetValue<TOptions>() where TOptions : IRendererOptions, new()
        {
            var type = typeof(TOptions);

            if (_options.TryGetValue(type, out var instance)) 
                return (TOptions) instance;
            
            instance = new TOptions();
            _options.Add(type, instance);

            return (TOptions)instance;
        }

        /// <inheritdoc />
        public override string ToString() => $"[{string.Join(",", _options.Values.Select(v => v.GetType().Name))}]";
    }
}