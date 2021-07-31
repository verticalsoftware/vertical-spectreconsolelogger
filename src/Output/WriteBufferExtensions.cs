using System;
using System.Runtime.CompilerServices;
using Vertical.SpectreLogger.Rendering;

namespace Vertical.SpectreLogger.Output
{
    public static class WriteBufferExtensions
    {
        public static void WriteMarkup(this IWriteBuffer buffer, string markup)
        {
            buffer.Append('[');
            buffer.Append(markup);
            buffer.Append(']');
        }

        public static void WriteMarkupClose(this IWriteBuffer buffer)
        {
            buffer.Append("[/]");
        }
        
        public static void WriteLine(this IWriteBuffer buffer)
        {
            buffer.Write(Environment.NewLine);
        }

        public static void WriteWhitespace(this IWriteBuffer buffer, int count = 1)
        {
            while (--count >= 0)
            {
                buffer.Append(' ');
            }
        }

        public static void Write(this IWriteBuffer buffer, in FormattedValue formattedValue)
        {
            buffer.Write(formattedValue.Value!, formattedValue.Markup);
        }
        
        public static void Write(this IWriteBuffer buffer, string content, string? markup = null)
        {
            if (markup != null)
            {
                buffer.WriteMarkup(markup);
            }
            
            foreach (var c in content)
            {
                Write(buffer, c);
            }
            
            if (markup != null)
            {
                buffer.WriteMarkupClose();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(this IWriteBuffer buffer, char c, int count = 1)
        {
            while (--count >= 0)
            {
                buffer.Append(c);
            }
        }
    }
}