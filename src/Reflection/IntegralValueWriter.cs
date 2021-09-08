using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Output;

namespace Vertical.SpectreLogger.Reflection
{
    internal class IntegralValueWriter : IDestructuredObjectWriter
    {
        private IntegralValueWriter()
        {
        }

        internal static readonly IDestructuredObjectWriter Default = new IntegralValueWriter();
        
        /// <inheritdoc />
        public void WriteValue(IWriteBuffer writeBuffer, LogLevelProfile profile, object value, int depth)
        {
            writeBuffer.WriteLogValue(profile, null, value);
        }
    }
}