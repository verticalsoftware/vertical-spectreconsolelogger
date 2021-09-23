using System;
using System.Linq;

namespace Vertical.SpectreLogger.Formatting
{
    /// <summary>
    /// Defines groups of integral/system types.
    /// </summary>
    public static class Types
    {
        /// <summary>
        /// Represents an array of types that are unsigned integers.
        /// </summary>
        public static readonly Type[] UnsignedIntegers =
        {
            typeof(byte),
            typeof(byte?),
            typeof(ushort),
            typeof(ushort?),
            typeof(uint),
            typeof(uint?),
            typeof(ulong),
            typeof(ulong?)
        };

        /// <summary>
        /// Represents an array of types that are signed integers.
        /// </summary>
        public static readonly Type[] SignedIntegers =
        {
            typeof(sbyte),
            typeof(sbyte?),
            typeof(short),
            typeof(short?),
            typeof(int),
            typeof(int?),
            typeof(long),
            typeof(long?)
        };
        
        /// <summary>
        /// Represents an array of types that are real numbers.
        /// </summary>
        public static readonly Type[] RealNumbers =
        {
            typeof(float),
            typeof(float?),
            typeof(double),
            typeof(double?),
            typeof(decimal),
            typeof(decimal?)
        };

        /// <summary>
        /// Represents an array of types that are numeric types.
        /// </summary>
        public static readonly Type[] Numerics = UnsignedIntegers
            .Concat(SignedIntegers)
            .Concat(RealNumbers)
            .ToArray();
        
        /// <summary>
        /// Represents an array of types that are characters.
        /// </summary>
        public static readonly Type[] Characters =
        {
            typeof(char),
            typeof(char?),
            typeof(string)
        };

        /// <summary>
        /// Represents an array of types that are pointers.
        /// </summary>
        public static readonly Type[] Pointers =
        {
            typeof(IntPtr),
            typeof(IntPtr?),
            typeof(UIntPtr),
            typeof(UIntPtr?)
        };
        
        /// <summary>
        /// Represents an array of types that are temporal values.
        /// </summary>
        public static readonly Type[] Temporal =
        {
            typeof(DateTime),
            typeof(DateTime?),
            typeof(DateTimeOffset),
            typeof(DateTimeOffset?),
            typeof(TimeSpan),
            typeof(TimeSpan?),
        };
    }
}