namespace Vertical.SpectreLogger.Destructuring
{
    /// <summary>
    /// Writes an object in destructured format.
    /// </summary>
    public interface IDestructuringWriter
    {
        /// <summary>
        /// Writes the start of an object.
        /// </summary>
        void WriteStartObject();
        
        /// <summary>
        /// Writes a property key/value.
        /// </summary>
        /// <param name="key">Property key</param>
        /// <param name="value">Property value</param>
        /// <returns></returns>
        bool WriteProperty(string key, object? value);
        
        /// <summary>
        /// Writes an integral value.
        /// </summary>
        /// <param name="value"></param>
        void WriteIntegral(object? value);
        
        /// <summary>
        /// Writes the end notation for an object.
        /// </summary>
        void WriteEndObject();
        
        /// <summary>
        /// Writes the start of an array.
        /// </summary>
        void WriteStartArray();
        
        /// <summary>
        /// Writes the end notation for an array.
        /// </summary>
        void WriteEndArray();
        
        /// <summary>
        /// Writes an element of a collection.
        /// </summary>
        /// <param name="value">Value to write</param>
        /// <returns>Whether the writer descended into </returns>
        bool WriteElement(object? value);
    }
}