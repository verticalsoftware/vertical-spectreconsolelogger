namespace Vertical.SpectreLogger.Core
{
    public interface ILogEventFilter
    {
        /// <summary>
        /// Determines whether a log event should be rendered.
        /// </summary>
        /// <param name="eventInfo"><see cref="LogEventInfo"/> to evaluate.</param>
        /// <returns>Whether render the event.</returns>
        bool Render(in LogEventInfo eventInfo);
    }
}