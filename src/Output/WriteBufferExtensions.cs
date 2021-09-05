using System;
using Vertical.SpectreLogger.Internal;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Templates;

namespace Vertical.SpectreLogger.Output
{
    /// <summary>
    /// Extends the <see cref="IWriteBuffer"/> interface.
    /// </summary>
    public static class WriteBufferExtensions
    {
        /// <summary>
        /// Writes a newline to the buffer.
        /// </summary>
        /// <param name="buffer">Buffer</param>
        public static void WriteLine(this IWriteBuffer buffer)
        {
            buffer.Write(Environment.NewLine);                
        }

        public static void EnqueueLine(this IWriteBuffer buffer)
        {
            buffer.Enqueue(Environment.NewLine);
        }
        
        /// <summary>
        /// Writes a log value to the buffer.
        /// </summary>
        /// <param name="buffer">Write buffer</param>
        /// <param name="profile">The profile that contains the styles and formatting to apply</param>
        /// <param name="templateSegment">The template segment</param>
        /// <param name="value">The value to write</param>
        /// <typeparam name="T">The value type</typeparam>
        public static void WriteLogValue<T>(
            this IWriteBuffer buffer,
            LogLevelProfile profile,
            TemplateSegment? templateSegment,
            T value)
            where T : notnull
        {
            var closeTag = WriteOpenMarkupTag(buffer, profile, value);
            var format = templateSegment?.CompositeFormatSpan ?? string.Empty;
            var formatString = "{0" + format + "}";
            var valueFormatted = string.Format(
                profile.FormatProvider,
                formatString,
                value);

            buffer.Write(valueFormatted);

            if (closeTag != null)
            {
                buffer.Write(closeTag);
            }
        }

        private static string? WriteOpenMarkupTag<T>(
            IWriteBuffer buffer,
            LogLevelProfile profile, 
            T value) 
            where T : notnull
        {
            var markup = profile.ValueStyles.GetValueOrDefault(value, null)
                   ??
                   profile.TypeStyles.GetValueOrDefault(value.GetType(), null);

            if (markup == null)
                return null;
            
            buffer.Write(markup);

            return "[/]";
        }
    }
}