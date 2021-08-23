using System;
using System.Collections.Generic;
using System.Linq;

namespace Vertical.SpectreLogger.Types
{
    public static class TypeConstants
    {
        public static readonly Type[] CharTypes = {typeof(char), typeof(string)};
        public static readonly Type[] TemporalTypes = {typeof(DateTime), typeof(DateTimeOffset), typeof(TimeSpan)};
        public static readonly Type[] PointerTypes = {typeof(IntPtr), typeof(UIntPtr)};
        public static readonly Type[] UnsignedIntegerTypes = {typeof(byte), typeof(ushort), typeof(uint), typeof(ulong)};
        public static readonly Type[] SignedIntegerTypes = {typeof(sbyte), typeof(ushort), typeof(uint), typeof(ulong)};
        public static readonly Type[] RealNumberTypes = {typeof(float), typeof(double), typeof(decimal)};
        public static readonly Type[] BooleanTypes = {typeof(bool)};
        public static readonly Type[] UniqueIdentifierTypes = {typeof(Guid)};
        public static readonly Type[] IntegerTypes = UnsignedIntegerTypes.Concat(SignedIntegerTypes).ToArray();
        public static readonly Type[] NumericTypes = IntegerTypes.Concat(RealNumberTypes).ToArray();

        internal static Type[] MakeGenericTypes(IEnumerable<Type> type)
        {
            var nullableType = typeof(Nullable<>);
            
            return type
                .Distinct()
                .Select(underlyingType => nullableType.MakeGenericType(underlyingType))
                .ToArray();
        }
    }
}