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
            typeof(ushort),
            typeof(uint),
            typeof(ulong)
        };

        public static readonly Type[] SignedIntegers =
        {
            typeof(sbyte),
            typeof(short),
            typeof(int),
            typeof(long)
        };

        public static readonly Type[] RealNumbers =
        {
            typeof(float),
            typeof(double),
            typeof(decimal)
        };

        public static readonly Type[] Numerics = UnsignedIntegers
            .Concat(SignedIntegers)
            .Concat(RealNumbers)
            .ToArray();
        
        public static readonly Type[] Characters =
        {
            typeof(char),
            typeof(string)
        };

        public static readonly Type[] Pointers =
        {
            typeof(IntPtr),
            typeof(UIntPtr)
        };

        public static readonly Type[] Temporal =
        {
            typeof(DateTime),
            typeof(DateTimeOffset),
            typeof(TimeSpan)
        };
    }
}