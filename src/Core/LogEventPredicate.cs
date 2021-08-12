namespace Vertical.SpectreLogger.Core
{
    /// <summary>
    /// Evaluates the given log event and returns whether or not to render the event.
    /// </summary>
    public delegate bool LogEventPredicate(in LogEventInfo eventInfo);
}