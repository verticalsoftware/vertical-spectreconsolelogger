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
        /// Appends a string.
        /// </summary>
        /// <param name="str">Content to render.</param>
        void Append(string str);

        /// <summary>
        /// Appends a single character.
        /// </summary>
        /// <param name="c">Character to append</param>
        void Append(char c);

        /// <summary>
        /// Clears the buffer of all content.
        /// </summary>
        void Clear();

        /// <summary>
        /// Flushes the content to the target.
        /// </summary>
        void Flush();
        
        /// <summary>
        /// Gets or sets the left-aligned margin to apply.
        /// </summary>
        int Margin { get; set; }
    }
}