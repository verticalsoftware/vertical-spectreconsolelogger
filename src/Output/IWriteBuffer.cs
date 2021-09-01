namespace Vertical.SpectreLogger.Output
{
    /// <summary>
    /// Provides an interface for writing content.
    /// </summary>
    public interface IWriteBuffer
    {
        /// <summary>
        /// Gets the number of characters to indent anytime a newline character is
        /// encountered.
        /// </summary>
        int Margin { get; }
        
        /// <summary>
        /// Gets the number of characters that have been written since the last
        /// new line character.
        /// </summary>
        int LinePosition { get; }

        /// <summary>
        /// Enqueues characters that are not written until any other characters
        /// are written.
        /// </summary>
        /// <param name="str">String value</param>
        /// <param name="startIndex">Starting index</param>
        /// <param name="length">Number of characters to write</param>
        void Enqueue(string str, int startIndex, int length);

        /// <summary>
        /// Writes a string or string portion.
        /// </summary>
        /// <param name="str">String value</param>
        /// <param name="startIndex">Starting index</param>
        /// <param name="length">Number of characters to write</param>
        void Write(string str, int startIndex, int length);
        
        /// <summary>
        /// Gets the length of the buffer.
        /// </summary>
        int Length { get; }
        
        /// <summary>
        /// Flushes the content of the buffer to an underlying output. 
        /// </summary>
        void Flush();

        /// <summary>
        /// Clears the buffer of all content.
        /// </summary>
        void Clear();
    }
}