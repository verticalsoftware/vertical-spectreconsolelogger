using Vertical.SpectreLogger.Options;

namespace Vertical.SpectreLogger.Output
{
    public static class WriteBufferExtensions
    {
        public static void Append(this IWriteBuffer writeBuffer,
            FormattingProfile profile,
            string content,
            string? markup = null)
        {
            if (markup != null)
            {
                writeBuffer.AppendUnescaped($"[{markup}]");
            }
            
            writeBuffer.Append(profile, content);

            if (markup != null)
            {
                writeBuffer.AppendUnescaped("[/]");
            }
        }

        public static void AppendMarkup(this IWriteBuffer writeBuffer, string markup) =>
            writeBuffer.AppendUnescaped($"[{markup}]");

        public static void AppendMarkupCloseTag(this IWriteBuffer writeBuffer) =>
            writeBuffer.AppendUnescaped("[/]");
    }
}