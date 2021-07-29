using System;
using System.Text;
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
            _span = span;
        }

        /// <inheritdoc />
        public string Template => throw new NotImplementedException();

        /// <inheritdoc />
        public void Format(IWriteBuffer buffer, ref LogEventInfo eventInfo)
        {
            buffer.Append(eventInfo.FormattingProfile, _span);
        }
    }
}