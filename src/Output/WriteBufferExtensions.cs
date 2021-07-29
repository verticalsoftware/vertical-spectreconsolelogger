using Vertical.SpectreLogger.Options;

namespace Vertical.SpectreLogger.Output
{
    internal static class WriteBufferExtensions
    {
        internal static void Append(this IWriteBuffer writeBuffer,
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
    }
}