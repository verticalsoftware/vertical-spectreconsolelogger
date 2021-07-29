using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.Rendering;

namespace Vertical.SpectreLogger
{
    /// <summary>
    /// Creates and maintains <see cref="SpectreLogger"/> instances.
    /// </summary>
    public class SpectreLoggerProvider : ILoggerProvider
    {
        private static readonly ConcurrentDictionary<string, ILogger> CachedLoggers = new();
        
        private readonly IOptions<SpectreLoggerOptions> _optionsProvider;
        private readonly ITemplateRendererProvider _rendererProvider;
        private readonly AsyncLocal<ImmutableStack<SpectreLoggerScope>> _asyncLocalScopes = new();
        private readonly IWriteBufferFactory _writeBufferFactory;

        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        /// <param name="optionsProvider">Options provider.</param>
        /// <param name="rendererProvider">Template format provider</param>
        /// <param name="writeBufferFactory">Write buffer factory</param>
        public SpectreLoggerProvider(IOptions<SpectreLoggerOptions> optionsProvider, 
            ITemplateRendererProvider rendererProvider,
            IWriteBufferFactory writeBufferFactory)
        {
            _optionsProvider = optionsProvider;
            _rendererProvider = rendererProvider;
            _writeBufferFactory = writeBufferFactory;
        }
        
        /// <inheritdoc />
        public void Dispose()
        {
            // Not implemented
        }

        private ImmutableStack<SpectreLoggerScope> LocalStack
        {
            get => _asyncLocalScopes.Value ?? ImmutableStack<SpectreLoggerScope>.Empty;
            set => _asyncLocalScopes.Value = value;
        }

        /// <inheritdoc />
        public ILogger CreateLogger(string categoryName)
        {
            return CachedLoggers.GetOrAdd(categoryName, catg => new SpectreLogger(
                this,
                _optionsProvider.Value,
                _writeBufferFactory,
                _rendererProvider,
                catg));
        }

        /// <summary>
        /// Gets the current scopes, in order.
        /// </summary>
        internal IEnumerable<object?> Scopes => _asyncLocalScopes.Value?
            .Reverse()
            .Select(scope => scope.State) ?? Enumerable.Empty<object>();

        /// <summary>
        /// Signals a scope is disposed.
        /// </summary>
        /// <param name="scope">Scope instance.</param>
        internal void ScopeDisposed(SpectreLoggerScope scope)
        {
            var stack = LocalStack;

            if (!ReferenceEquals(stack.Peek(), scope))
                return;

            LocalStack = stack.Pop();
        }

        /// <summary>
        /// Begins tracking a new scope.
        /// </summary>
        /// <param name="state">State</param>
        /// <returns></returns>
        internal IDisposable BeginLoggerScope<T>(T state)
        {
            var scope = new SpectreLoggerScope(this, state);
            var stack = LocalStack;

            LocalStack = stack.Push(scope);

            return scope;
        }
    }
}