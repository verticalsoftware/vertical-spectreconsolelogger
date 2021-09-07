using System.Text.RegularExpressions;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.Templates;

namespace Vertical.SpectreLogger.Rendering
{
    /// <summary>
    /// Controls the margin in the template rendering.
    /// </summary>
    [Template(@"{Margin(?<_mode>[+->])(?<_value>\d+)}")]
    public class MarginControlRenderer : ITemplateRenderer
    {
        private readonly string _mode;
        private readonly int _value;

        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        /// <param name="match">Match object</param>
        public MarginControlRenderer(Match match)
        {
            _mode = match.Groups["_mode"].Value;
            _value = int.Parse(match.Groups["_value"].Value);
        }
        
        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventContext context)
        {
            switch (_mode)
            {
                case "-":
                    buffer.Margin -= _value;
                    break;
                
                case "+":
                    buffer.Margin += _value;
                    break;
                
                default:
                    buffer.Margin = _value;
                    break;
            }    
        }
    }
}