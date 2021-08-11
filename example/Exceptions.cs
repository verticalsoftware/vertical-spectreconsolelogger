using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Vertical.SpectreLogger;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Rendering;

namespace SpectreLoggerExample
{
    
    [Demo("All about exceptions")]
    public class Exceptions
    {
        [Demo("Demonstrates output using default settings")]
        public void DefaultSettings()
        {
            var logger = LoggerFactory
                .Create(builder => builder.AddSpectreConsole())
                .CreateLogger<Exceptions>();
            
            try
            {
                var _ = Array.Empty<int>().First();
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "An exception has occurred");
            }
        }

        [Demo("Demonstrates shortening path and module names/limits the stack trace")]
        public void ShortenPathAndModules()
        {
            var logger = LoggerFactory.Create(builder => builder
                    .AddSpectreConsole(options =>
                    {
                        options.ConfigureProfile(LogLevel.Error ,profile =>
                        {
                            profile.ConfigureRenderer<ExceptionRenderer.Options>(opt =>
                            {
                                opt.SourcePathFormatter = Path.GetFileName;
                                opt.MethodNameFormatter = name => name.Split('.').Last();
                                opt.MaxStackFrames = 5;
                                opt.RenderParameterNames = false;
                            });
                        });
                    }))
                .CreateLogger<Exceptions>();

            try
            {
                Task.Run(() => Array.Empty<int>().First()).Wait();
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "An exception has occurred");
            }
        }
    }
}