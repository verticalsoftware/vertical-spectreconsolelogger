namespace Vertical.SpectreLogger.Destructuring
{
    public interface IDestructuringWriter
    {
        void WriteStartObject();
        bool WriteProperty(string key, object? value);
        void WriteIntegral(object? value);
        void WriteEndObject();
        void WriteStartArray();
        void WriteEndArray();
        bool WriteElement(object? value);
    }
}