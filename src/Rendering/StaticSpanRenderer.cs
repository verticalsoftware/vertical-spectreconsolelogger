using System;
using System.Text;
using Spectre.Console;
using Vertical.SpectreLogger.Output;

namespace Vertical.SpectreLogger.Rendering
{
    /// <summary>
    /// Internal renderer that writes static strings to the buffer.
    /// </summary>
    internal class StaticSpanRenderer : ITemplateRenderer
    {
        private readonly string _span;

        internal StaticSpanRenderer(string span)
        {
            _span = span.EscapeMarkup();
        }

        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventInfo eventInfo)
        {
            buffer.Write(_span);
        }

        /// <inheritdoc />
        public override string ToString() => $"Static span=\"{_span}\"";
    }
}