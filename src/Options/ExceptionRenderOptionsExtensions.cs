using Vertical.SpectreLogger.Rendering;

namespace Vertical.SpectreLogger.Options
{
    public static class ExceptionRenderOptionsExtensions
    {
        /// <summary>
        /// Hides method parameter signatures in stack traces.
        /// </summary>
        /// <param name="options">Options</param>
        /// <returns><see cref="ExceptionRenderer.Options"/></returns>
        public static ExceptionRenderer.Options HideMethodParameters(this ExceptionRenderer.Options options)
        {
            options.RenderParameterTypes = false;
            options.RenderParameterNames = false;
            return options;
        }

        /// <summary>
        /// Renders the short name of exceptions in stack traces.
        /// </summary>
        /// <param name="options">Options</param>
        /// <returns><see cref="ExceptionRenderer.Options"/></returns>
        public static ExceptionRenderer.Options RenderShortExceptionNames(this ExceptionRenderer.Options options)
        {
            options.ExceptionNameFormatter = type => type.Name;
            return options;
        }

        /// <summary>
        /// Renders the name of the method without its declaring type(s) or namespace in stack traces.
        /// </summary>
        /// <param name="options">Options</param>
        /// <returns><see cref="ExceptionRenderer.Options"/></returns>
        public static ExceptionRenderer.Options RenderShortMethodNames(this ExceptionRenderer.Options options)
        {
            options.MethodNameFormatter = method =>
            {
                var lastIndexOfDot = method.LastIndexOf('.');
                return lastIndexOfDot > -1
                    ? method.Substring(lastIndexOfDot + 1)
                    : method;
            };
            return options;
        }

        /// <summary>
        /// Sets the number of stack frames to render to zero.
        /// </summary>
        /// <param name="options">Options</param>
        /// <returns><see cref="ExceptionRenderer.Options"/></returns>
        public static ExceptionRenderer.Options HideStackTrace(this ExceptionRenderer.Options options)
        {
            options.MaxStackFrames = 0;
            return options;
        }
    }
}