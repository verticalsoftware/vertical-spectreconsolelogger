using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Output;

namespace Vertical.SpectreLogger.Rendering
{
    /// <summary>
    /// Renders exceptions.
    /// </summary>
    public class ExceptionRenderer : ITemplateRenderer
    {
        /// <inheritdoc />
        public string Template => "{Exception(:(?<options>[^}]+))?}";

        /// <inheritdoc />
        public void Format(IWriteBuffer buffer, ref LogEventInfo eventInfo)
        {
            var exception = eventInfo.Exception;

            if (exception == null)
                return;
            
            var profile = eventInfo.FormattingProfile;
            var options = profile.GetRenderingOptions<ExceptionRenderingOptions>();
        }
    }
}