using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Internal;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.Templates;

namespace Vertical.SpectreLogger.Rendering
{
    internal sealed class RendererPipeline : IRendererPipeline
    {
        private readonly Dictionary<LogLevel, IReadOnlyList<ITemplateRenderer>> _pipelines;
        private readonly ObjectPool<IWriteBuffer> _bufferPool;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="optionsProvider">Options provider for <see cref="SpectreLoggerOptions"/></param>
        /// <param name="rendererBuilder">Object that builds renderers.</param>
        /// <param name="consoleWriter">Console writer implementation</param>
        public RendererPipeline(
            IOptions<SpectreLoggerOptions> optionsProvider,
            ITemplateRendererBuilder rendererBuilder,
            IConsoleWriter consoleWriter)
        {
            var options = optionsProvider.Value;
            
            _pipelines = options
                .LogLevelProfiles
                .ToDictionary(
                    entry => entry.Key, 
                    entry => CreatePipeline(rendererBuilder, entry.Value));

            _bufferPool = new DefaultObjectPool<IWriteBuffer>(
                new WriteBufferPooledObjectPolicy(consoleWriter),
                options.MaxPooledBuffers);
        }

        /// <inheritdoc />
        public void Render(in LogEventContext logEventContext)
        {
            var renderers = _pipelines[logEventContext.LogLevel];
            var count = renderers.Count;
            var buffer = _bufferPool.Get();

            try
            {
                for (var c = 0; c < count; c++)
                {
                    renderers[c].Render(buffer, logEventContext);
                }

                buffer.Margin = 0;

                if (buffer.LinePosition != 0)
                {
                    buffer.WriteLine();
                }
            }
            finally
            {
                _bufferPool.Return(buffer);   
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