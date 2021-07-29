using Vertical.SpectreLogger.Output;

namespace Vertical.SpectreLogger.Rendering
{
    public class LogLevelRenderer : ITemplateRenderer
    {
        /// <inheritdoc />
        public string Template => "{LogLevel}";

        /// <inheritdoc />
        public void Format(IWriteBuffer buffer, ref LogEventInfo eventInfo)
        {
            var profile = eventInfo.FormattingProfile;
            var displayValue = profile.LogLevelDisplay;

            buffer.Append(profile, displayValue, profile.LogLevelMarkup);
        }
    }
}