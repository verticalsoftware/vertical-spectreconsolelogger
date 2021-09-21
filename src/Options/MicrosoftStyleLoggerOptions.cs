using Microsoft.Extensions.Logging;

namespace Vertical.SpectreLogger.Options
{
    public static class MicrosoftStyleLoggerOptions
    {
        /// <summary>
        /// Configures the provider output similar to Microsoft's console logger implementation.
        /// </summary>
        /// <param name="config">Builder object</param>
        /// <returns><see cref="SpectreLoggingBuilder"/></returns>
        public static SpectreLoggingBuilder UseMicrosoftConsoleStyle(this SpectreLoggingBuilder config)
        {
            config.ConfigureProfiles(profile => profile
                .AddTypeFormatter<LogLevel>((_, obj, __) =>
                    (LogLevel) obj! switch
                    {
                        LogLevel.Trace => "trce",
                        LogLevel.Debug => "dbug",
                        LogLevel.Information => "info",
                        LogLevel.Warning => "warn",
                        LogLevel.Error => "fail",
                        LogLevel.Critical => "crit",
                        _ => string.Empty
                    })
                .OutputTemplate = "{LogLevel}: {CategoryName}{Margin=6}{NewLine}{Message}{NewLine}{Exception}");
            
            config.ConfigureProfile(LogLevel.Information, profile => profile.AddTypeStyle<LogLevel>("[green]"));
            config.ConfigureProfile(LogLevel.Warning, profile => profile.AddTypeStyle<LogLevel>("[gold3_1]"));
            config.ConfigureProfile(LogLevel.Error, profile => profile.AddTypeStyle<LogLevel>("[red1]"));
            config.ConfigureProfile(LogLevel.Critical, profile => profile.AddTypeStyle<LogLevel>("[white on red1]"));
            
            return config;
        }
    }
}