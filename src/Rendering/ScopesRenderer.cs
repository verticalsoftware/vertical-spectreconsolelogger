using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Output;
using Vertical.SpectreLogger.Types;
using Vertical.SpectreLogger.Utilities;

namespace Vertical.SpectreLogger.Rendering
{
    [Template("{Scopes}")]
    public partial class ScopesRenderer : ITemplateRenderer
    {
        /// <inheritdoc />
        public void Render(IWriteBuffer buffer, in LogEventInfo eventInfo)
        {
            var scopes = eventInfo.Scopes;
            var profile = eventInfo.FormattingProfile;
            var options = profile.GetRendererOptions<ScopesRendererOptions>();
            var length = scopes.Length;

            if (length == 0)
            {
                if (options?.DefaultFormat != null)
                {
                    buffer.Write(options.DefaultFormat);
                    return;
                }
            }

            var separator = string.Empty;
            var appendSeparator = options?.Separator ?? string.Empty;
            var preFormatEvaluated = false;

            for (var c = 0; c < length; c++)
            {
                buffer.Write(separator);

                var scopeValue = scopes[c];
                
                switch (scopeValue)
                {
                    case null when true == options?.RenderNullScopes:
                        RenderPreformat(buffer, options, ref preFormatEvaluated);
                        buffer.WriteFormattedValue(
                            NullLogValue.Default,
                            profile);
                        break;
                    
                    case null:
                        continue;
                    
                    default:
                        RenderPreformat(buffer, options, ref preFormatEvaluated);
                        buffer.WriteStateValue(profile, scopeValue);
                        break;
                }

                separator = appendSeparator;
            }

            if (options?.PostRenderFormat != null)
            {
                buffer.Write(options.PostRenderFormat);
            }
        }

        private static void RenderPreformat(
            IWriteBuffer buffer, 
            ScopesRendererOptions? options, 
            ref bool evaluated)
        {
            if (evaluated)
                return;

            evaluated = true;
            
            if (options?.PreRenderFormat != null)
            {
                buffer.Write(options.PreRenderFormat);
            }
        }
    }
}