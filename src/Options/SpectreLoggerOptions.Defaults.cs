using Microsoft.Extensions.Logging;

namespace Vertical.SpectreLogger.Options
{
    public partial class SpectreLoggerOptions
    {
        public SpectreLoggerOptions()
        {
            var options = this;

            options.ConfigureProfiles(profile =>
            {
                profile.OutputTemplate = "{LogLevel,-7}:{CategoryName:F}{Margin:8}{NewLine}Hello SpectreLogger!";
            });

            options.ConfigureProfile(LogLevel.Trace, profile =>
            {
                profile.AddValueFormatter(LogLevel.Trace, _ => "[Trace]");
            });
            
            options.ConfigureProfile(LogLevel.Debug, profile =>
            {
                profile.AddValueFormatter(LogLevel.Debug, _ => "[Debug]");
            });
            
            options.ConfigureProfile(LogLevel.Information, profile =>
            {
                profile.AddValueFormatter(LogLevel.Information, _ => "[Info]");
            });
            
            options.ConfigureProfile(LogLevel.Warning, profile =>
            {
                profile.AddValueFormatter(LogLevel.Warning, _ => "[Warn]");
            });
            
            options.ConfigureProfile(LogLevel.Error, profile =>
            {
                profile.AddValueFormatter(LogLevel.Error, _ => "[Error]");
            });
            
            options.ConfigureProfile(LogLevel.Critical, profile =>
            {
                profile.AddValueFormatter(LogLevel.Critical, _ => "[Crit]");
            });
        }
    }
}