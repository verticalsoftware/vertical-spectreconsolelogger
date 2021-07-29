using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace Vertical.SpectreLogger.Options
{
    public static class SpectreLoggerDefaults
    {
        /// <summary>
        /// Defines the default output template.
        /// </summary>
        public const string OutputTemplate = "[{LogLevel}]: {Message}{Exception}";

        internal static void ConfigureDefaults(SpectreLoggerOptions options)
        {
            options.ConfigureProfile(LogLevel.Trace, profile =>
            {
                profile.BaseMarkup = Color.Grey35.ToMarkup();
                profile.OutputTemplate = OutputTemplate;
                profile.AddTypeStyle<object>(Color.Purple_1.ToMarkup());
                profile.LogLevelDisplay = "Trc";
            });
            
            options.ConfigureProfile(LogLevel.Debug, profile =>
            {
                profile.BaseMarkup = Color.Grey50.ToMarkup();
                profile.OutputTemplate = OutputTemplate;
                profile.AddTypeStyle<object>(Color.Turquoise2.ToMarkup());
                profile.LogLevelDisplay = "Dbg";
            });
            
            options.ConfigureProfile(LogLevel.Information, profile =>
            {
                profile.BaseMarkup = Color.Grey93.ToMarkup();
                profile.OutputTemplate = OutputTemplate;
                profile.AddTypeStyle<object>(Color.Cyan1.ToMarkup());
                profile.LogLevelDisplay = "Inf";
            });
            
            options.ConfigureProfile(LogLevel.Warning, profile =>
            {
                profile.BaseMarkup = Color.LightGoldenrod1.ToMarkup();
                profile.OutputTemplate = OutputTemplate;
                profile.AddTypeStyle<object>(Color.White.ToMarkup());
                profile.LogLevelDisplay = "Wrn";
            });
            
            options.ConfigureProfile(LogLevel.Error, profile =>
            {
                profile.BaseMarkup = Color.Red1.ToMarkup();
                profile.OutputTemplate = OutputTemplate;
                profile.AddTypeStyle<object>("bold");
                profile.LogLevelDisplay = "Err";
            });
            
            options.ConfigureProfile(LogLevel.Critical, profile =>
            {
                profile.BaseMarkup = $"{Color.White.ToMarkup()} on {Color.Red1.ToMarkup()}";
                profile.OutputTemplate = OutputTemplate;
                profile.AddTypeStyle<object>("bold");
                profile.LogLevelDisplay = "Crt";
            });
            
            options.ConfigureProfiles(profile => profile.AddValueFormatter<object>(obj => obj?.ToString()));
        }
    }
}