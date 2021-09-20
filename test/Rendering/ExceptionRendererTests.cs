using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VerifyXunit;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Rendering;
using Vertical.SpectreLogger.Tests.Infrastructure;
using Xunit;

namespace Vertical.SpectreLogger.Tests.Rendering
{
    [UsesVerify]
    public class ExceptionRendererTests
    {
        [Fact]
        public Task RenderWritesExpected()
        {
            var output = RendererTestHarness.Capture(
                config => ConfigureBaseOptions(config),
                logger => logger.LogError(ExceptionHelper.GetAggregateException(), "Error occurred"));

            return Verifier.Verify(output);
        }

        private static void ConfigureBaseOptions(
            SpectreLoggingBuilder config,
            Action<ExceptionRenderer.Options>? exceptionConfig = null)
        {
            config.ConfigureProfiles(profile =>
            {
                profile.OutputTemplate = "{LogLevel}: {Message}{Exception+}";
                profile.TypeFormatters.Clear();
                profile.TypeStyles.Clear();
                profile.DefaultLogValueStyle = null;

                if (exceptionConfig != null)
                {
                    profile.ConfigureOptions<ExceptionRenderer.Options>(exceptionConfig);
                }
            });
        }
    }
}