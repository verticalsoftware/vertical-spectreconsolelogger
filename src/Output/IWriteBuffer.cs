using System;
using Vertical.SpectreLogger.Options;

namespace Vertical.SpectreLogger.Output
{
    /// <summary>
    /// Represents an object that buffers output to the console.
    /// </summary>
    public interface IWriteBuffer : IDisposable
    {
        /// <summary>
        /// Appends content to the buffer.
        /// </summary>
        /// <param name="profile">Current formatting profile</param>
        /// <param name="str">String content to append.</param>
        void Append(FormattingProfile profile, string str);

        /// <summary>
        /// Appends whitespace content to the buffer.
        /// </summary>
        /// <param name="count">The number of whitespace characters to append.</param>
        void AppendWhitespace(int count = 1);

        /// <summary>
        /// Appends a newline to the buffer.
        /// </summary>
        void AppendLine(FormattingProfile profile);

        /// <summary>
        /// Appends markup to the buffer.
        /// </summary>
        /// <param name="content">Markup to append.</param>
        void AppendUnescaped(string content);

        /// <summary>
        /// Flushes the content to the target.
        /// </summary>
        void Flush();
    }
}