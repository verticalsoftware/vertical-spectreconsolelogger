using System;
using Vertical.SpectreLogger.Output;

namespace Vertical.SpectreLogger.Utilities
{
    public static partial class WriteBufferExtensions
    {
        /// <summary>
        /// Determines if content has been written past the set margin.
        /// </summary>
        /// <param name="buffer">Buffer</param>
        /// <returns>Boolean</returns>
        public static bool AtMargin(this IWriteBuffer buffer) => buffer.CharPosition == buffer.Margin;

        /// <summary>
        /// Writes a new line to the buffer.
        /// </summary>
        /// <param name="buffer">Buffer</param>
        public static void WriteLine(this IWriteBuffer buffer) => buffer.Write(Environment.NewLine);
    }
}