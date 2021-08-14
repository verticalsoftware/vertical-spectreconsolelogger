
using System;
using System.Collections.Generic;

namespace Vertical.SpectreLogger.Tests.Infrastructure
{
    public class EqualityComparerFunction<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> _comparer;
        private readonly Func<T, int>? _hashFunction;

        public EqualityComparerFunction(Func<T, T, bool> comparer, Func<T, int>? hashFunction = null)
        {
            _comparer = comparer;
            _hashFunction = hashFunction;
        }
        
        /// <inheritdoc />
        public bool Equals(T? x, T? y)
        {
            if (ReferenceEquals(x, null) && ReferenceEquals(y, null))
                return true;

            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                return false;

            return _comparer(x, y);
        }

        /// <inheritdoc />
        public int GetHashCode(T obj)
        {
            return _hashFunction?.Invoke(obj) ?? obj!.GetHashCode();
        }
    }
}