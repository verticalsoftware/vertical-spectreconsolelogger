namespace Vertical.SpectreLogger.Core
{
    /// <summary>
    /// Defines a delegate that receives log event data and returns a boolean
    /// indicating whether or not the event should be filtered from the output.
    /// </summary>
    public delegate bool LogEventFilterDelegate(in LogEventContext context);
}