using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Formatting;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.Types;

namespace Vertical.SpectreLogger.Utilities
{
    public static partial class WriteBufferExtensions
    {
        
        private static void WriteDestructuredValue(this IWriteBuffer buffer,
            object value,
            FormattingProfile profile,
            TemplateContext? templateContext,
            IFormatter? formatter,
            FormattingOptions options)
        {
        }
        
        private static void WriteDestructuredValue(this IWriteBuffer buffer,
            object? value,
            FormattingProfile profile,
            TemplateContext? templateContext,
            IFormatter? formatter,
            FormattingOptions options,
            int depth)
        {
            switch (value)
            {
                case null:
                    buffer.WriteFormattedValue(NullLogValue.Default, profile, templateContext, formatter, options);
                    break;
                
                
            }
        }
    }
}