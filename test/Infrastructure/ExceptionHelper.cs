using System;
using System.Linq;
using System.Threading.Tasks;

namespace Vertical.SpectreLogger.Tests.Infrastructure
{
    // Note - changing this class may alter Verify outputs
    public static class ExceptionHelper
    {
        public static Exception GetAggregateException()
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