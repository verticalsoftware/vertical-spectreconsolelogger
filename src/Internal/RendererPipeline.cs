using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.Rendering;
using Vertical.SpectreLogger.Templates;

namespace Vertical.SpectreLogger.Internal
{
    internal sealed class RendererPipeline : IRendererPipeline
    {
        private readonly Dictionary<LogLevel, IReadOnlyList<ITemplateRenderer>> _pipelines;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="optionsProvider">Options provider for <see cref="SpectreLoggerOptions"/></param>
        /// <param name="rendererBuilder">Object that builds renderers.</param>
        public RendererPipeline(
            IOptions<SpectreLoggerOptions> optionsProvider,
            ITemplateRendererBuilder rendererBuilder)
        {
            _pipelines = optionsProvider
                .Value
                .LogLevelProfiles
                .ToDictionary(
                    entry => entry.Key, 
                    entry => CreatePipeline(rendererBuilder, entry.Value));
        }

        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventContext logEventContext)
        {
            var renderers = _pipelines[logEventContext.LogLevel];
            var count = renderers.Count;

            for (var c = 0; c < count; c++)
            {
                renderers[c].Render(buffer, logEventContext);
            }
        }

        private static IReadOnlyList<ITemplateRenderer> CreatePipeline(
            ITemplateRendererBuilder rendererBuilder, 
            LogLevelProfile profile)
        {
            return string.IsNullOrEmpty(profile.OutputTemplate) 
                ? new[] {new StaticSpanRenderer($"LogLevelProfile '{profile.LogLevel}' has no defined output template.")} 
                : rendererBuilder.GetRenderers(profile.OutputTemplate!);
        }
    }
}