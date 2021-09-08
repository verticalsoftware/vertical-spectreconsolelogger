using System;
using System.Collections.Concurrent;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Output;

namespace Vertical.SpectreLogger.Reflection
{
    internal interface IDestructuredObjectWriter
    {
        void WriteValue(
            IWriteBuffer writeBuffer,
            LogLevelProfile profile,
            object value,
            int depth);
    }
}