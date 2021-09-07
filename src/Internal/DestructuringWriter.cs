using Vertical.SpectreLogger.Formatting;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.Reflection;

namespace Vertical.SpectreLogger.Internal
{
    internal static class DestructuringWriter
    {
        internal static void WriteValue(
            IWriteBuffer buffer,
            LogLevelProfile profile,
            DestructuringOptions options,
            object? value)
        {
        }
        
        private static void WriteValue(
            IWriteBuffer buffer,
            LogLevelProfile profile,
            DestructuringOptions options,
            object? value,
            int depth)
        {
            switch (value)
            {
                case null:
                    buffer.WriteLogValue(profile, null, NullValue.Default);
                    break;
                
                case var dictionary when DestructuringHelpers.IsDictionary(value.GetType()):
                    break;
            }
        }

        private static void WriteDictionary(
            IWriteBuffer buffer,
            LogLevelProfile profile,
            DestructuringOptions options,
            object dictionary,
            int depth)
        {
            buffer.Write('{');
            
            
            
            buffer.Write('}');
        }
    }
}