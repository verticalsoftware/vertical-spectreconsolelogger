namespace Vertical.SpectreLogger.Options
{
    public static class LogLevelRenderingOptionsExtensions
    {
        /// <summary>
        /// Sets the name to display for the log level.
        /// </summary>
        /// <param name="options">Options</param>
        /// <param name="displayName">Name to display</param>
        /// <returns><see cref="CategoryNameRenderingOptions"/></returns>
        public static LogLevelRenderingOptions SetDisplayName(this LogLevelRenderingOptions options,
            string displayName)
        {
            options.Formatter = _ => displayName;
            return options;
        }
    }
}