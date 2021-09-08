using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Output;

namespace Vertical.SpectreLogger.Reflection
{
    internal class StructuredObjectWriter : IDestructuredObjectWriter
    {
        private readonly DestructuredObjectWriterProvider _provider;

        internal StructuredObjectWriter(DestructuredObjectWriterProvider provider)
        {
            _provider = provider;
        }
        
        /// <inheritdoc />
        public void WriteValue(IWriteBuffer writeBuffer, LogLevelProfile profile, object value, int depth)
        {
            throw new System.NotImplementedException();
        }
    }
}