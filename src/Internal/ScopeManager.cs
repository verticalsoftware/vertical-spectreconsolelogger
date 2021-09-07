using System;
using System.Threading;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Formatting;

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
            var newScope = new LoggerScope(this, current, value ?? (object)NullValue.Default);

            _localScope.Value = newScope;

            return newScope;
        }

        /// <summary>
        /// Gets the current scope values.
        /// </summary>
        internal IScopeValues GetValues() => ScopeValues.Create(_localScope.Value);
    }
}