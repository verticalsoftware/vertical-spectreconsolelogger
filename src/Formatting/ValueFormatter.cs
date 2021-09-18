using System;

namespace Vertical.SpectreLogger.Formatting
{
    /// <summary>
    /// Formats a value.
    /// </summary>
    public delegate string ValueFormatter(string? format, object? obj, IFormatProvider? provider);
}