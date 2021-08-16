namespace Vertical.SpectreLogger.Core
{
    public interface IFormatter
    {
        string Format(string format, object value);
    }
}