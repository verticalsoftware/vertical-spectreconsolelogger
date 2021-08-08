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
        private const string MyTemplate = @"{NewLine(\?)?(?::([-]?)(\d+)?(!)?)}";
        
        private readonly bool _conditional;
        private readonly int? _margin;
        private readonly bool _assign;

        public NewLineRenderer(Match matchContext)
        {
            _conditional = matchContext.Groups[1].Success;
            _margin = matchContext.Groups[2].Success
                ? int.Parse(matchContext.Groups[2].Value)
                : null;
            _assign = matchContext.Groups[3].Success;
        }

        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventInfo eventInfo)
        {
            if (_margin.HasValue)
            {
                buffer.Margin = _assign
                    ? _margin.Value
                    : buffer.Margin + _margin.Value;
            }

            if (_conditional && buffer.AtMargin)
                return;
         
            buffer.WriteLine();
        }

        /// <inheritdoc />
        public override string ToString() => $"NewLine{(_conditional ? "?" : "")} ";
    }
}