namespace Vertical.SpectreLogger.Output
{
    /// <summary>
    /// Provides an interface to IAnsiConsole
    /// </summary>
    public interface IAnsiConsoleWriter
    {
        /// <summary>
        /// Writes content to the underlying console.
        /// </summary>
        /// <param name="content">Content to write.</param>
        void Write(string content);
    }
}