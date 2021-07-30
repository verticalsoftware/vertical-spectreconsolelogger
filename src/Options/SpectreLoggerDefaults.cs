using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Vertical.SpectreLogger.Internal;

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
                profile.LogLevel = LogLevel.Trace;
                profile.BaseMarkup = Color.Grey35.ToMarkup();
                profile.OutputTemplate = OutputTemplate;
                profile.AddTypeStyle<object>(Color.DodgerBlue3.ToMarkup());
                profile.LogLevelDisplay = "Trc";
            });
            
            options.ConfigureProfile(LogLevel.Debug, profile =>
            {
                profile.LogLevel = LogLevel.Debug;
                profile.BaseMarkup = Color.Grey50.ToMarkup();
                profile.OutputTemplate = OutputTemplate;
                profile.AddTypeStyle<object>(Color.Turquoise2.ToMarkup());
                profile.LogLevelDisplay = "Dbg";
            });
            
            options.ConfigureProfile(LogLevel.Information, profile =>
            {
                profile.LogLevel = LogLevel.Information;
                profile.BaseMarkup = Color.Grey93.ToMarkup();
                profile.OutputTemplate = OutputTemplate;
                profile.AddTypeStyle<object>(Color.Cyan1.ToMarkup());
                profile.LogLevelDisplay = "Inf";
            });
            
            options.ConfigureProfile(LogLevel.Warning, profile =>
            {
                profile.LogLevel = LogLevel.Warning;
                profile.BaseMarkup = Color.LightGoldenrod1.ToMarkup();
                profile.OutputTemplate = OutputTemplate;
                profile.AddTypeStyle<object>(Color.Cyan1.ToMarkup());
                profile.LogLevelDisplay = "Wrn";
            });
            
            options.ConfigureProfile(LogLevel.Error, profile =>
            {
                profile.LogLevel = LogLevel.Error;
                profile.BaseMarkup = Color.Red1.ToMarkup();
                profile.OutputTemplate = OutputTemplate;
                profile.AddTypeStyle<object>(Color.White.ToMarkup());
                profile.LogLevelDisplay = "Err";
            });
            
            options.ConfigureProfile(LogLevel.Critical, profile =>
            {
                profile.LogLevel = LogLevel.Critical;
                profile.BaseMarkup = Color.Red1.ToMarkup();
                profile.OutputTemplate = OutputTemplate;
                profile.AddTypeStyle<object>(Color.White.ToMarkup());
                profile.LogLevelDisplay = "Crt";
                profile.LogLevelMarkup = "white on red1";
            });

            options.ConfigureProfiles(profile =>
            {
                profile.ConfigureRendererOptions<ExceptionRenderingOptions>(opt =>
                {
                    opt.ExceptionMessageMarkup = Color.Grey93.ToMarkup();
                    opt.ExceptionNameFormatter = type => type.FullName!;
                    opt.ExceptionNameMarkup = Color.Grey93.ToMarkup();
                    opt.SourceLineNumberMarkup = Color.DodgerBlue1.ToMarkup();
                    opt.MaxStackFrames = 5;
                    opt.MethodNameMarkup = Color.DarkOrange.ToMarkup();
                    opt.ParameterNameMarkup = Color.Grey93.ToMarkup();
                    opt.ParameterTypeMarkup = Color.DodgerBlue1.ToMarkup();
                    opt.SourcePathFormatter = path => path;
                    opt.SourcePathMarkup = Color.Grey50.ToMarkup();
                    opt.StackFrameMarkup = Color.Grey50.ToMarkup();
                    opt.UnwindAggregateExceptions = true;
                });
            });
            
            options.ConfigureProfiles(profile => profile.AddValueFormatter<object>(obj => obj?.ToString()));
        }
    }
}