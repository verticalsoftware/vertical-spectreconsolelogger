using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Output;

namespace Vertical.SpectreLogger.Rendering
{
    internal sealed class StaticSpanRenderer : ITemplateRenderer
    {
        private readonly string _content;

        internal StaticSpanRenderer(string content)
        {
            _content = content;
        }
        
        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventInfo logEventInfo)
        {
            buffer.Write(_content, 0, _content.Length);
        }
    }
}