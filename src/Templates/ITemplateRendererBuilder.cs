using System.Collections.Generic;
using Vertical.SpectreLogger.Core;

namespace Vertical.SpectreLogger.Templates
{
    /// <summary>
    /// Defines an object that produces an ordered sequence of template renderers.
    /// </summary>
    public interface ITemplateRendererBuilder
    {
        /// <summary>
        /// Builds an ordered collection of template renderers.
        /// </summary>
        /// <param name="templateString">Template string.</param>
        /// <returns>List of <see cref="ITemplateRenderer"/> objects.</returns>
        IReadOnlyList<ITemplateRenderer> GetRenderers(string templateString);
    }
}