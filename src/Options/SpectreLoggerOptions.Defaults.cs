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
                profile.OutputTemplate = "{LogLevel,-7}: {CategoryName:F}{Margin:9}{NewLine}Hello SpectreLogger!";
                profile.DefaultFormatter = obj => obj.ToString() ?? string.Empty;
            });

            options.ConfigureProfile(LogLevel.Trace, profile =>
            {
                profile.AddValueFormatter(LogLevel.Trace, "[Trace]");
            });
            
            options.ConfigureProfile(LogLevel.Debug, profile =>
            {
                profile.AddValueFormatter(LogLevel.Debug, "[Debug]");
            });
            
            options.ConfigureProfile(LogLevel.Information, profile =>
            {
                profile.OutputTemplate = "[green]{LogLevel,-7}[/][grey35]: {CategoryName:F}{Margin:9}{NewLine}Hello SpectreLogger![/]";
                profile.AddValueFormatter(LogLevel.Information, "[Info]");
            });
            
            options.ConfigureProfile(LogLevel.Warning, profile =>
            {
                profile.AddValueFormatter(LogLevel.Warning, "[Warn]");
            });
            
            options.ConfigureProfile(LogLevel.Error, profile =>
            {
                profile.AddValueFormatter(LogLevel.Error, "[Error]");
            });
            
            options.ConfigureProfile(LogLevel.Critical, profile =>
            {
                profile.AddValueFormatter(LogLevel.Critical, "[Crit]");
            });
        }
    }
}