using System.Collections.Generic;
using Vertical.SpectreLogger.Core;

namespace Vertical.SpectreLogger.Infrastructure
{
    public interface ITemplateRendererFactory
    {
        /// <summary>
        /// Creates an ordered list of renderers determined by the output template.
        /// </summary>
        /// <param name="outputTemplate">Output template.</param>
        /// <returns>A collection of <see cref="ITemplateRenderer"/> objects that are in invocation
        /// order.</returns>
        IReadOnlyList<ITemplateRenderer> CreatePipeline(string outputTemplate);
    }
}