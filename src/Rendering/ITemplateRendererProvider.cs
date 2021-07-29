using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Vertical.SpectreLogger.Rendering
{
    /// <summary>
    /// Locates the appropriate formatters to use given a template string.
    /// </summary>
    public interface ITemplateRendererProvider
    {
        /// <summary>
        /// Gets the formatters for a provided log level.
        /// </summary>
        IEnumerable<ITemplateRenderer> GetRenderers(LogLevel logLevel);
    }
}