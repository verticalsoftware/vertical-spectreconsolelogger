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

        [Fact]
        public Task RenderHidesParameterTypes()
        {
            var output = RendererTestHarness.Capture(
                config => ConfigureBaseOptions(config, ex => ex.ShowParameterTypes = false),
                logger => logger.LogError(ExceptionHelper.GetAggregateException(), "Error occurred"));

            return Verifier.Verify(output);
        }
        
        [Fact]
        public Task RenderHidesParameterNames()
        {
            var output = RendererTestHarness.Capture(
                config => ConfigureBaseOptions(config, ex => ex.ShowParameterNames = false),
                logger => logger.LogError(ExceptionHelper.GetAggregateException(), "Error occurred"));

            return Verifier.Verify(output);
        }
        
        [Fact]
        public Task RenderHidesSourceLocations()
        {
            var output = RendererTestHarness.Capture(
                config => ConfigureBaseOptions(config, ex => ex.ShowSourceLocations = false),
                logger => logger.LogError(ExceptionHelper.GetAggregateException(), "Error occurred"));

            return Verifier.Verify(output);
        }
        
        [Fact]
        public Task RenderHidesSourcePaths()
        {
            var output = RendererTestHarness.Capture(
                config => ConfigureBaseOptions(config, ex => ex.ShowSourcePaths = false),
                logger => logger.LogError(ExceptionHelper.GetAggregateException(), "Error occurred"));

            return Verifier.Verify(output);
        }
        
        [Fact]
        public Task RenderHidesAggregateInnerExceptions()
        {
            var output = RendererTestHarness.Capture(
                config => ConfigureBaseOptions(config, ex => ex.UnwindInnerExceptions = false),
                logger => logger.LogError(ExceptionHelper.GetAggregateException(), "Error occurred"));

            return Verifier.Verify(output);
        }

        [Fact]
        public Task RenderLimitsStackFrames()
        {
            var output = RendererTestHarness.Capture(
                config => ConfigureBaseOptions(config, ex => ex.MaxStackFrames = 10),
                logger => logger.LogError(ExceptionHelper.GetAggregateException(), "Error occurred"));

            return Verifier.Verify(output);
        }

        private static void ConfigureBaseOptions(
            SpectreLoggingBuilder config,
            Action<ExceptionRenderer.Options>? exceptionConfig = null)
        {
            config.ConfigureProfiles(profile =>
            {
                profile.OutputTemplate = "{LogLevel}: {Message}{NewLine+}{Exception}";
                profile.TypeFormatters.Clear();
                profile.TypeStyles.Clear();
                profile.DefaultLogValueStyle = null;

                profile.AddTypeFormatter<ExceptionRenderer.SourceLocationValue>((_, _) => "{line}");

                if (exceptionConfig != null)
                {
                    profile.ConfigureOptions(exceptionConfig);
                }
            });
        }
    }
}