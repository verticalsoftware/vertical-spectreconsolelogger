using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Vertical.SpectreLogger;
using Vertical.SpectreLogger.Options;
using Vertical.SpectreLogger.Rendering;

namespace Exceptions
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = LoggerFactory
                .Create(logging => logging.AddSpectreConsole(specLogger =>
                {
                    specLogger.ConfigureProfile(LogLevel.Error, profile =>
                    {
                        profile.ConfigureOptions<ExceptionRenderer.Options>(opt =>
                        {
                            // Don't show more than 8 stack frames
                            opt.MaxStackFrames = 8;

                            // Show parameter types but not names
                            opt.ShowParameterTypes = true;
                            opt.ShowParameterNames = false;

                            // Show inner exceptions
                            opt.UnwindInnerExceptions = true;
                        });
                        
                        // Make parameter types pink
                        profile.AddTypeStyle<ExceptionRenderer.ParameterTypeValue>($"[{Color.HotPink}]");
                        
                        // Make line numbers red 
                        profile.AddTypeStyle<ExceptionRenderer.SourceLocationValue>($"[{Color.Red1}]");
                        
                        // Trim the directory
                        profile.AddTypeFormatter<ExceptionRenderer.SourceDirectoryValue>((_, arg) =>
                        {
                            var path = arg.Value;
                            
                            var split = path!.Split(Path.DirectorySeparatorChar);

                            return Path.Combine(
                                "...",
                                string.Join(Path.DirectorySeparatorChar, split[^2..]));
                        });
                        
                        // Show the exception class name only
                        profile.AddTypeFormatter<ExceptionRenderer.ExceptionNameValue>((_, arg) => arg.Value.Name);
                    });
                }))
                .CreateLogger<Program>();
            
            logger.LogError(GetException(), "A simulated error has occurred!");
        }
        
        private static Exception GetException()
        {
            try
            {
                Task.Run(() => Array.Empty<string>().First()).Wait();
            }
            catch (Exception exception)
            {
                return exception;
            }

            return new Exception();
        }
    }
}
