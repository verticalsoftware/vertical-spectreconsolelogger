using System;

namespace Vertical.SpectreLogger.Output
{
    /// <summary>
    /// Represents an object that buffers data to the final output.
    /// </summary>
    public interface IWriteBuffer : IDisposable
    {
        /// <summary>
        /// Writes a string to the buffer.
        /// </summary>
        /// <param name="str">String to write.</param>
        void Write(string str);

        /// <summary>
        /// Writes a portion of a string to the buffer.
        /// </summary>
        /// <param name="str">String to write.</param>
        /// <param name="index">Starting position in the string.</param>
        /// <param name="length">The number of characters to write.</param>
        void Write(string str, int index, int length);

        /// <summary>
        /// Writes a character to the buffer.
        /// </summary>
        /// <param name="c">Character to write.</param>
        /// <param name="count">The number of times to repeat the operation.</param>
        void Write(char c, int count = 1);

        /// <summary>
        /// Flushes the content of the buffer to the final output.
        /// </summary>
        void Flush();

        /// <summary>
        /// Clears all content from the buffer.
        /// </summary>
        void Clear();
        
        /// <summary>
        /// Gets or sets the number of characters to indent on new lines of output.
        /// </summary>
        int Margin { get; set; }

        /// <summary>
        /// Gets the number of character written since the last line feed (\n).
        /// </summary>
        int CharPosition { get; }

        /// <summary>
        /// Gets the content length of the current buffer.
        /// </summary>
        int Length { get; }

        /// <summary>
        /// Gets a portion of the content within the buffer.
        /// </summary>
        /// <param name="startIndex">Starting index.</param>
        /// <param name="length">Length to retrieve.</param>
        /// <returns><see cref="string"/></returns>
        string ToString(int startIndex, int length);
    }
}