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
        /// <param name="eventInfo"><see cref="LogEventInfo"/> that describes the event.</param>
        /// <returns>True if the event should be filtered from the output; false otherwise.</returns>
        bool Filter(in LogEventInfo eventInfo);
    }
}