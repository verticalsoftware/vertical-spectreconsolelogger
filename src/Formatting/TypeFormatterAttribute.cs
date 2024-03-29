﻿using System;

namespace Vertical.SpectreLogger.Formatting
{
    /// <summary>
    /// Attributes used to describe the type a formatter supports.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TypeFormatterAttribute : Attribute
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="type">The type this instance provides formatting for.</param>
        public TypeFormatterAttribute(Type type)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
        }

        /// <summary>
        /// Gets the type to associate with the formatter.
        /// </summary>
        public Type Type { get; }
    }
}