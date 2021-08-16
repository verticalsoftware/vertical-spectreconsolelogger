using System.Text.RegularExpressions;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.Utilities;

namespace Vertical.SpectreLogger.Rendering
{
    [Template(@"{NewLine(?<margin>\?)?}")]
    public class NewLineRenderer : ITemplateRenderer
    {
        private readonly bool _onlyIfNotAtMargin;

        public NewLineRenderer(Match matchContext) : this(matchContext.Groups["margin"].Success)
        {
        }

        public NewLineRenderer(bool onlyIfNotAtMargin)
        {
            _onlyIfNotAtMargin = onlyIfNotAtMargin;
        }
        
        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventInfo eventInfo)
        {
            if (buffer.AtMargin() && _onlyIfNotAtMargin)
                return;
            
            buffer.WriteLine();
        }
    }
}