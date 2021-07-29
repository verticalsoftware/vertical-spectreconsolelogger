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
        /// Appends markup to the buffer.
        /// </summary>
        /// <param name="markup">Markup to append.</param>
        void AppendUnescaped(string markup);

        /// <summary>
        /// Flushes the content to the target.
        /// </summary>
        void Flush();
    }
}