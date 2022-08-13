using Vertical.SpectreLogger.Formatting;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.Reflection;

namespace Vertical.SpectreLogger.Destructuring
{
    /// <summary>
    /// Handles rendering log values in a semi-JSON like format.
    /// </summary>
    internal class DestructuringWriter : IDestructuringWriter
    {
        private readonly IWriteBuffer _buffer;
        private readonly LogLevelProfile _profile;
        private readonly DestructuringOptions _options;
        private readonly int _availableDepth;
        private readonly int _indentation;
        private int _innerCount;
        
        private DestructuringWriter(
            IWriteBuffer buffer,
            LogLevelProfile profile)
        {
            _buffer = buffer;
            _profile = profile;
            _options = profile.ConfiguredOptions.GetOptions<DestructuringOptions>();
            _availableDepth = _options.MaxDepth;
            _indentation = _options.IndentSpaces;
        }

        private DestructuringWriter(
            IWriteBuffer buffer,
            LogLevelProfile profile,
            DestructuringOptions options,
            int availableDepth)
        {
            _buffer = buffer;
            _profile = profile;
            _options = options;
            _availableDepth = availableDepth;
            _indentation = (options.MaxDepth - _availableDepth) * _options.IndentSpaces;
        }
        
        public static void Write(
            IWriteBuffer buffer,
            LogLevelProfile profile,
            object value)
        {
            var writer = new DestructuringWriter(buffer, profile);
            
            writer.WriteValue(value);
        }

        public bool WriteProperty(string key, object? value) => WriteNode(key, value, _options.MaxProperties);

        /// <inheritdoc />
        public bool WriteElement(object? value)
        {
            return WriteNode(null, value, _options.MaxCollectionItems);
        }

        public void WriteIntegral(object? value)
        {
            _buffer.WriteLogValue(_profile, null, value ?? NullValue.Default);
        }

        public void WriteStartObject() => WriteStartSection("{");

        public void WriteEndObject() => WriteEndSection("}");

        /// <inheritdoc />
        public void WriteStartArray() => WriteStartSection("[[");

        /// <inheritdoc />
        public void WriteEndArray() => WriteEndSection("]]");

        private void WriteStartSection(string c)
        {
            _buffer.Write(c);
            
            if (_options.WriteIndented)
            {
                _buffer.Margin += _indentation;
            }
        }

        private void WriteEndSection(string c)
        {
            if (_options.WriteIndented)
            {
                _buffer.Margin -= _indentation;
                if (_innerCount > 0)
                {
                    _buffer.WriteLine();
                }
            }
            
            _buffer.Write(c);
        }

        private void WriteValue(object value)
        {
            var valueWriter = CompiledWriterCache.GetInstance(value.GetType(), WriteIntegralDelegate);

            valueWriter(this, value);
        }
        
        private bool WriteNode(string? key, object? value, int maxCount)
        {
            if (_innerCount++ == maxCount)
            {
                _buffer.Write(", ...");
                return false;
            }

            if (_innerCount == 1 && _options.WriteIndented)
            {
                _buffer.WriteLine();
            }

            if (_innerCount > 1)
            {
                _buffer.Write(", ");
                
                if (_options.WriteIndented)
                {
                    _buffer.WriteLine();
                }
            }

            if (key != null)
            {
                _buffer.WriteLogValue(_profile, null, new DestructuredKeyValue(key), k =>
                {
                    _buffer.Write(k);
                    _buffer.Write(": ");
                });
            }

            if (_availableDepth < 0)
            {
                _buffer.WriteLogValue(_profile, null, value?.ToString() ?? NullValue.Default.ToString());
                return true;
            }

            new DestructuringWriter(
                _buffer,
                _profile,
                _options,
                _availableDepth - 1
            ).WriteValue(value ?? NullValue.Default);
            
            return true;
        }

        private static readonly CompiledWriter WriteIntegralDelegate = (writer, value) => writer.WriteIntegral(value);
    }
}