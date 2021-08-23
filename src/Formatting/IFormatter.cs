namespace Vertical.SpectreLogger.Formatting
{
    /// <summary>
    /// Defines the interface of an object that formats values to a particular
    /// representation.
    /// </summary>
    public interface IFormatter
    {
        string Format(string format, object value);
    }
}