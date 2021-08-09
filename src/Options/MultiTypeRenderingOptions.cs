using System;
using System.Collections.Generic;

namespace Vertical.SpectreLogger.Options
{
    public abstract class MultiTypeRenderingOptions
    {
        /// <summary>
        /// Gets or sets style markup to apply before rendering a value of a specific type.
        /// The key is the type name.
        /// </summary>
        public Dictionary<Type, string> TypeStyles { get; } = new();
        
        /// <summary>
        /// Gets or sets the markup to apply before rendering a value where the type is
        /// not found in <see cref="TypeStyles"/>.
        /// </summary>
        public string? DefaultTypeStyle { get; set; }

        /// <summary>
        /// Gets or sets the markup to apply before rendering a specific value of a type. The
        /// key is a tuple combining the type and value.
        /// </summary>
        public Dictionary<(Type, object), string> ValueStyles { get; } = new();

        /// <summary>
        /// Gets a dictionary of functions that convert object value types
        /// to specific string representations.
        /// </summary>
        public Dictionary<Type, Func<object?, string?>> TypeFormatters { get; } = new();

        /// <summary>
        /// Gets or sets the formatting function to use if one cannot be
        /// found in <see cref="TypeFormatters"/>.
        /// </summary>
        public Func<object, string>? DefaultTypeFormatter { get; set; } = obj => obj.ToString() ?? string.Empty;
    }
}