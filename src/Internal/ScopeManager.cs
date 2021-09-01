using System;
using System.Threading;
using Vertical.SpectreLogger.Core;

namespace Vertical.SpectreLogger.Internal
{
    /// <summary>
    /// Manages logging scopes.
    /// </summary>
    internal sealed class ScopeManager
    {
        private readonly AsyncLocal<LoggerScope?> _localScope = new();
        
        /// <summary>
        /// Signaled when a scope is disposed.
        /// </summary>
        /// <param name="scope">Scope</param>
        internal void ScopeDisposed(LoggerScope scope)
        {
            _localScope.Value = scope.PreviousScope;
        }
        
        /// <summary>
        /// Begins a new scope.
        /// </summary>
        /// <param name="value">Scope value</param>
        /// <typeparam name="T">Value type</typeparam>
        /// <returns><see cref="IDisposable"/></returns>
        internal IDisposable BeginScope<T>(T value)
        {
            var current = _localScope.Value;
            var newScope = new LoggerScope(this, current, value);

            _localScope.Value = newScope;

            return newScope;
        }

        /// <summary>
        /// Gets the deferred scope collection.
        /// </summary>
        /// <returns><see cref="GetScopeValueCollection"/></returns>
        internal ScopeValueCollection GetScopeValueCollection() => new(_localScope.Value);
    }
}