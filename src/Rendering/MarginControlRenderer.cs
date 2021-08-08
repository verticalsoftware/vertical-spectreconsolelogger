using System;
using System.Text.RegularExpressions;
using Vertical.SpectreLogger.Output;

namespace Vertical.SpectreLogger.Rendering
{
    /// <summary>
    /// Controls offsets of the margin.
    /// </summary>
    [Template(MyTemplate)]
    public class MarginControlRenderer : ITemplateRenderer
    {
        private const string MyTemplate = @"{Margin:(-?\d+)(!)?}";

        private enum SetMode { Assign, OffsetIncrement, OffsetDecrement };
        
        private readonly int _value;
        private readonly bool _assign;

        public MarginControlRenderer(Match matchContext)
        {
            _value = int.Parse(matchContext.Groups[1].Value);
            _assign = matchContext.Groups[2].Success;
        }
        
        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventInfo eventInfo)
        {
            buffer.Margin = _assign
                ? _value
                : buffer.Margin + _value;
        }

        /// <inheritdoc />
        public override string ToString() => $"Margin control={_value}";
    }
}