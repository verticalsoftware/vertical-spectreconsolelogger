namespace Vertical.SpectreLogger.Output
{
    /// <summary>
    /// Provides an interface to IAnsiConsole
    /// </summary>
    public interface IConsoleWriter
    {
        /// <summary>
        /// When implemented, resets the output device for the next operation.
        /// </summary>
        void ResetLine();
        
        /// <summary>
        /// Writes content to the underlying console.
        /// </summary>
        /// <param name="content">Content to write.</param>
        void Write(string content);
    }
}