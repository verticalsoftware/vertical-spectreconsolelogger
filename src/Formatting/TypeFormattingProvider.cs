using System;
using System.Collections.Generic;

namespace Vertical.SpectreLogger.Formatting
{
    /// <summary>
    /// Represents a provider that returns a <see cref="TypeFormatter"/>
    /// </summary>
    internal class TypeFormattingProvider : IFormatProvider
    {
        private readonly TypeFormatter _formatter;

        internal TypeFormattingProvider(Dictionary<Type, ICustomFormatter> typeFormatters)
        {
            _formatter = new TypeFormatter(typeFormatters);
        }
        
        /// <inheritdoc />
        public object? GetFormat(Type? formatType)
        {
            return typeof(ICustomFormatter) == formatType
                ? _formatter
                : null;
        }
    }
}