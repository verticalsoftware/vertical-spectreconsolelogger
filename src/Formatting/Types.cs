using System;
using System.Linq;

namespace Vertical.SpectreLogger.Formatting
{
    /// <summary>
    /// Defines groups of integral/system types.
    /// </summary>
    public static class Types
    {
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

        public static readonly Type[] RealNumbers =
        {
            typeof(float),
            typeof(float?),
            typeof(double),
            typeof(double?),
            typeof(decimal),
            typeof(decimal?)
        };

        public static readonly Type[] Numerics = UnsignedIntegers
            .Concat(SignedIntegers)
            .Concat(RealNumbers)
            .ToArray();
        
        public static readonly Type[] Characters =
        {
            typeof(char),
            typeof(char?),
            typeof(string)
        };

        public static readonly Type[] Pointers =
        {
            typeof(IntPtr),
            typeof(IntPtr?),
            typeof(UIntPtr),
            typeof(UIntPtr?)
        };

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