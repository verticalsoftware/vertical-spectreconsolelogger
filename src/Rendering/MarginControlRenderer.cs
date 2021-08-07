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
        private const string MyTemplate = @"{Margin:([+-])(\d+)}";
        
        private readonly int _offset;

        public MarginControlRenderer(Match matchContext)
        {
            var multiplier = matchContext.Groups[1].Value == "-" ? -1 : 1;
            _offset = int.Parse(matchContext.Groups[2].Value) * multiplier;
        }
        
        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventInfo eventInfo)
        {
            buffer.Margin += _offset;
        }
    }
}