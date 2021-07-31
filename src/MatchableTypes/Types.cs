using System;
using System.Collections.Generic;
using System.Linq;

namespace Vertical.SpectreLogger.MatchableTypes
{
    public static class Types
    {
        /// <summary>
        /// Defines signed and unsigned integer types.
        /// </summary>
        public static IEnumerable<Type> IntegerTypes { get; } = new[]
        {
            typeof(short),
            typeof(int),
            typeof(long),
            typeof(float),
            typeof(double),
            typeof(decimal),
            typeof(ushort),
            typeof(uint),
            typeof(ulong)
        };

        /// <summary>
        /// Defines floating point types.
        /// </summary>
        public static IEnumerable<Type> FloatingPointTypes { get; } = new[]
        {
            typeof(float),
            typeof(double),
            typeof(decimal),
        };

        /// <summary>
        /// Defines pointer types.
        /// </summary>
        public static IEnumerable<Type> PointerTypes { get; } = new[]
        {
            typeof(IntPtr),
            typeof(UIntPtr)
        };

        /// <summary>
        /// Combines integer types, floating point types, and pointer types.
        /// </summary>
        public static IEnumerable<Type> NumericTypes { get; } = IntegerTypes
            .Concat(FloatingPointTypes)
            .Concat(PointerTypes);

        /// <summary>
        /// Defines the byte and sbyte types.
        /// </summary>
        public static IEnumerable<Type> ByteTypes { get; } = new[] {typeof(byte), typeof(sbyte)};

        /// <summary>
        /// Defines most of the System namespace value types.
        /// </summary>
        public static IEnumerable<Type> ValueTypes { get; } = IntegerTypes
            .Concat(FloatingPointTypes)
            .Concat(PointerTypes)
            .Concat(NumericTypes)
            .Concat(ByteTypes)
            .Concat(new[] {typeof(bool), typeof(char)});

        /// <summary>
        /// Defines character types.
        /// </summary>
        public static IEnumerable<Type> CharacterTypes { get; } = new[]
        {
            typeof(string),
            typeof(char)
        };
    }
}