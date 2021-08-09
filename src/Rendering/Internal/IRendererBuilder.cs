using Microsoft.Extensions.Logging;

namespace Vertical.SpectreLogger.Rendering.Internal
{
    /// <summary>
    /// Locates the appropriate formatters to use given a template string.
    /// </summary>
    public interface IRendererBuilder
    {
        /// <summary>
        /// Gets the formatters for a provided log level.
        /// </summary>
        // ReSharper disable once ReturnTypeCanBeEnumerable.Global
        ITemplateRenderer[] GetRenderers(LogLevel logLevel);
    }
}