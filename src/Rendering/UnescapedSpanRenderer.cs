using System;
using Vertical.SpectreLogger.Output;

namespace Vertical.SpectreLogger.Rendering
{
    /// <summary>
    /// Internal renderer that writes static strings to the buffer.
    /// </summary>
    internal class UnescapedSpanRenderer : ITemplateRenderer
    {
        private readonly string _span;

        internal UnescapedSpanRenderer(string span)
        {
            _span = span;
        }

        /// <inheritdoc />
        public string Template => throw new NotImplementedException();

        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, ref LogEventInfo eventInfo)
        {
            buffer.AppendUnescaped(_span);
        }
    }
}