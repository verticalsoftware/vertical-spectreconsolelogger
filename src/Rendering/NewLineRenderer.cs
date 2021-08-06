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
        private const string MyTemplate = @"{NewLine(\?)?(?::(\d+)(!)?)?}";
        private readonly bool _conditional;
        private readonly int _indent;
        private readonly bool _setMargin;

        public NewLineRenderer(string? templateContext = null)
        {
            if (templateContext == null)
                return;
            
            var match = Regex.Match(templateContext, MyTemplate);

            _conditional = match.Groups[1].Success;
            _indent = match.Groups[2].Success ? int.Parse(match.Groups[3].Value) : 0;
            _setMargin = match.Groups[3].Value == "!";
        }

        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventInfo eventInfo)
        {
            if (_conditional && buffer.AtMargin)
                return;
            
            buffer.WriteLine();

            if (_indent > 0)
            {
                buffer.WriteWhitespace(_indent);
            }

            if (_setMargin)
            {
                buffer.Margin = _indent;
            }
        }
    }
}