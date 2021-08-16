using System.Text.RegularExpressions;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Output;

namespace Vertical.SpectreLogger.Rendering
{
    [Template(@"{Margin:(?<mode>[+-])?(?<value>\d+)}")]
    public class MarginControlRenderer : ITemplateRenderer
    {
        private enum Mode
        {
            Offset,
            Set
        };

        private readonly Mode _mode;
        private int _value;
        
        public MarginControlRenderer(Match match)
        {
            _mode = match.Groups["mode"].Value switch
            {
                "+" => Mode.Offset,
                "-" => Mode.Offset,
                _ => Mode.Set
            };
            _value = int.Parse(match.Groups["value"].Value);
        }

        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventInfo eventInfo)
        {
            switch (_mode)
            {
                case Mode.Set:
                    buffer.Margin = _value;
                    break;
                
                case Mode.Offset:
                    buffer.Margin = buffer.Margin + _value;
                    break;
            }
        }
    }
}