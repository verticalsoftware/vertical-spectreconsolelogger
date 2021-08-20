using System.Text.RegularExpressions;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Output;

namespace Vertical.SpectreLogger.Rendering
{
    [Template(@"{Style:(.+)}")]
    public class StyleControlRenderer : ITemplateRenderer
    {
        public static readonly ITemplateRenderer CloseTag = new StyleControlRenderer("/");
        
        private readonly string _style;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="match">Match object</param>
        public StyleControlRenderer(Match match) : this(match.Groups[1].Value)
        {
        }

        internal StyleControlRenderer(string style) => _style = $"[{style}]";
        
        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventInfo eventInfo)
        {
            buffer.Write(_style);
        }
    }
}