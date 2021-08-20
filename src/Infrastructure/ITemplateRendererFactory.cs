using System.Collections.Generic;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Options;

namespace Vertical.SpectreLogger.Infrastructure
{
    public interface ITemplateRendererFactory
    {
        /// <summary>
        /// Creates an ordered list of renderers determined by the output template.
        /// </summary>
        /// <param name="profile">Formatting profile</param>
        /// <returns>A collection of <see cref="ITemplateRenderer"/> objects that are in invocation
        /// order.</returns>
        IReadOnlyList<ITemplateRenderer> CreatePipeline(FormattingProfile profile);
    }
}