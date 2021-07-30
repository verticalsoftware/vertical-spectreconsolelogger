using System;
using System.Text.RegularExpressions;
using Vertical.SpectreLogger.Output;

namespace Vertical.SpectreLogger.Rendering
{
    /// <summary>
    /// Renderer that writes newlines to the buffer.
    /// </summary>
    [Template(MyTemplate)]
    public class NewLineRenderer : ITemplateRenderer
    {
        private const string MyTemplate = @"{NewLine(:(\d+))?}";
        private readonly int _indent;

        public NewLineRenderer(string? templateContext = null)
        {
            if (templateContext == null)
                return;
            
            var match = Regex.Match(templateContext, MyTemplate);

            if (int.TryParse(match.Groups[2].Value, out var i))
            {
                _indent = i;
            }
        }

        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, ref LogEventInfo eventInfo)
        {
            buffer.WriteLine();

            if (_indent > 0)
            {
                buffer.WriteWhitespace(_indent);
            }
        }
    }
}