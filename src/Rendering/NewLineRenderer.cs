using System;
using System.Text;
using Vertical.SpectreLogger.Output;

namespace Vertical.SpectreLogger.Rendering
{
    /// <summary>
    /// Renderer that writes newlines to the buffer.
    /// </summary>
    public class NewLineRenderer : ITemplateRenderer
    {
        internal static ITemplateRenderer Default { get; } = new NewLineRenderer();
        
        /// <inheritdoc />
        public string Template => "{NewLine}";

        /// <inheritdoc />
        public void Format(IWriteBuffer buffer, ref LogEventInfo eventInfo)
        {
            buffer.Append(eventInfo.FormattingProfile, Environment.NewLine);
        }
    }
}