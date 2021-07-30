using Spectre.Console;
using Vertical.SpectreLogger.Output;

namespace Vertical.SpectreLogger.Rendering
{
    [Template("{LogLevel}")]
    public class LogLevelRenderer : ITemplateRenderer
    {
        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, ref LogEventInfo eventInfo)
        {
            var profile = eventInfo.FormattingProfile;
            var displayValue = profile.LogLevelDisplay;

            buffer.Write(displayValue.EscapeMarkup(), profile.LogLevelMarkup);
        }
    }
}