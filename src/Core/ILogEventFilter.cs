namespace Vertical.SpectreLogger.Core
{
    /// <summary>
    /// Determines when events should be rendered.
    /// </summary>
    public interface ILogEventFilter
    {
        /// <summary>
        /// Determines whether the given event should be rendered.
        /// </summary>
        /// <param name="eventContext"><see cref="LogEventContext"/> that describes the event.</param>
        /// <returns>True if the event should be printed; false to ignore the event.</returns>
        bool Filter(in LogEventContext eventContext);
    }
}