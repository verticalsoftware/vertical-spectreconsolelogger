using System;
using System.Text.RegularExpressions;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.Utilities;

namespace Vertical.SpectreLogger.Rendering
{
    [Template(@"{NewLine(?<margin>\?)?(?<enqueue>\?)?}")]
    public class NewLineRenderer : ITemplateRenderer
    {
        private readonly bool _onlyIfNotAtMargin;
        private readonly bool _enqueue;

        public NewLineRenderer(Match matchContext) : this(matchContext.Groups["margin"].Success, 
            matchContext.Groups["enqueue"].Success)
        {
        }

        public NewLineRenderer(bool onlyIfNotAtMargin, bool enqueue)
        {
            _onlyIfNotAtMargin = onlyIfNotAtMargin;
            _enqueue = enqueue;
        }
        
        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventInfo eventInfo)
        {
            if (_enqueue)
            {
                buffer.Enqueue(Environment.NewLine);
                return;
            }
            
            if (buffer.AtMargin() && _onlyIfNotAtMargin)
                return;
            
            buffer.WriteLine();
        }
    }
}