using System.Text.RegularExpressions;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.Templates;

namespace Vertical.SpectreLogger.Rendering
{
    /// <summary>
    /// Renderer that writes newlines to the buffer.
    /// </summary>
    public class NewLineRenderer : ITemplateRenderer
    {
        private readonly bool _atMargin;

        [TemplateProvider]
        public static readonly Template Template = new()
        {
            CustomPattern = $"{{NewLine(\\?)?}}"
        };

        public NewLineRenderer(Match match)
        {
            _atMargin = match.Groups[1].Success;
        }

        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventInfo eventInfo)
        {
            if (!buffer.AtMargin || !_atMargin)
            {
                buffer.WriteLine();
            }
        }
    }
}