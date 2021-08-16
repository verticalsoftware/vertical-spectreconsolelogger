using System.Collections.Generic;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Options;

namespace Vertical.SpectreLogger.Infrastructure
{
    internal class RuntimeFormattingProfile : FormattingProfile
    {
        /// <inheritdoc />
        internal RuntimeFormattingProfile(
            FormattingProfile formattingProfile,
            IReadOnlyList<ITemplateRenderer> rendererPipeline) 
            : base(formattingProfile.LogLevel)
        {
            RendererPipeline = rendererPipeline;
            formattingProfile.CopyTo(this);
        }

        internal IReadOnlyList<ITemplateRenderer> RendererPipeline { get; }
    }
}