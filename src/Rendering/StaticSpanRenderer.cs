using Spectre.Console;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Output;

namespace Vertical.SpectreLogger.Rendering
{
    internal class StaticSpanRenderer : ITemplateRenderer
    {
        private readonly string _value;

        internal StaticSpanRenderer(string value) => _value = value;

        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventInfo eventInfo)
        {
            buffer.Write(_value);
        }
    }
}