using System;
using System.Collections.Concurrent;
using Vertical.SpectreLogger.Formatting;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.Reflection;

namespace Vertical.SpectreLogger.Destructuring
{
    public class DestructuringWriter : IDestructuringWriter
    {
        private static readonly ConcurrentDictionary<Type, CompiledWriter> 
            CachedValueWriters = new();

        private readonly IWriteBuffer _buffer;
        private readonly LogLevelProfile _profile;
        private readonly DestructuringOptions _options;
        private readonly int _availableDepth;
        private int _innerCount;

        private DestructuringWriter(
            IWriteBuffer buffer,
            LogLevelProfile profile,
            int? depth = null)
        {
            _buffer = buffer;
            _profile = profile;
            _options = profile.RendererOptions.GetOptions<DestructuringOptions>();
            _availableDepth = depth.GetValueOrDefault(_options.MaxDepth);
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
        }
        
        public static void Write(
            IWriteBuffer buffer,
            LogLevelProfile profile,
            object value)
        {
            var writer = new DestructuringWriter(buffer, profile);
            
            writer.WriteValue(value);
        }

        public bool WriteProperty(string key, object? value) => WriteNode(key, value);

        /// <inheritdoc />
        public bool WriteElement(object? value) => WriteNode(null, value);
        
        public void WriteIntegral(object? value)
        {
            _buffer.WriteLogValue(_profile, null, value ?? NullValue.Default);
        }
        
        public void WriteStartObject()
        {
            _buffer.Write('{');
        }
        
        public void WriteEndObject()
        {
            _buffer.Write('}');
        }

        /// <inheritdoc />
        public void WriteStartArray()
        {
            _buffer.Write("[[");
        }

        /// <inheritdoc />
        public void WriteEndArray()
        {
            _buffer.Write("]]");
        }

        private void WriteValue(object value)
        {
            var valueWriter = CachedValueWriters.GetOrAdd(value.GetType(), type => 
                CompiledWriterFactory.CreateWriter(type) ?? WriteIntegralDelegate);

            valueWriter(this, value);
        }
        
        private bool WriteNode(string? key, object? value)
        {
            if (_innerCount++ == _options.MaxProperties)
            {
                _buffer.Write(", ...");
                return false;
            }

            if (_innerCount > 1)
            {
                _buffer.Write(", ");
            }

            if (key != null)
            {
                _buffer.Write(key);
                _buffer.Write(": ");
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