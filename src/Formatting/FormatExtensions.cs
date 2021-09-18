using System;
using System.Diagnostics.CodeAnalysis;

namespace Vertical.SpectreLogger.Formatting
{
    internal static class FormatExtensions
    {
        [SuppressMessage("ReSharper", "FormatStringProblem")]
        internal static string Format(
            this object obj,
            IFormatProvider formatProvider,
            string? format = "")
        {
            return format != null
                ? string.Format(formatProvider, "{0" + format + "}", obj)
                : string.Format(formatProvider, "{0}", obj);
        }
    }
}