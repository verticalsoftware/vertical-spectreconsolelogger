using Microsoft.Extensions.Logging;
using Spectre.Console;
using Vertical.SpectreLogger.PseudoTypes;
using Vertical.SpectreLogger.Rendering;

namespace Vertical.SpectreLogger.Options
{
    public partial class SpectreLoggerOptions
    {
        /// <summary>
        /// Defines the default output template.
        /// </summary>
        public const string OutputTemplate = "{LogLevel,-5} : {Margin:8}{Message}{Exception:NewLine?}";

        private static void ConfigureDefaults(SpectreLoggerOptions options)
        {
            options.ConfigureProfile(LogLevel.Trace, profile =>
            {
                profile.LogLevel = LogLevel.Trace;

                profile.BaseEventStyle = Color.Grey35.ToMarkup();
                profile.SetDefaultTypeStyle(Color.Purple4_1.ToMarkup());
                profile.SetLogLevelDisplayName("Trace");
                profile.AddTypeStyle(Types.NumericTypes, Color.DarkMagenta.ToMarkup());
                profile.AddTypeStyle(Types.CharacterTypes, Color.DeepSkyBlue4_2.ToMarkup());
                profile.AddTypeStyle<LogLevel>(profile.BaseEventStyle!);
                profile.AddValueStyle(true, Color.DarkGreen.ToMarkup());
                profile.AddValueStyle(false, Color.DarkRed_1.ToMarkup());
                profile.AddTypeStyle<NullValue>(Color.DarkOrange3.ToMarkup());
            });

            options.ConfigureProfile(LogLevel.Debug, profile =>
            {
                profile.LogLevel = LogLevel.Debug;
                profile.BaseEventStyle = Color.Grey50.ToMarkup();
                profile.SetDefaultTypeStyle(Color.Purple4_1.ToMarkup());
                profile.SetLogLevelDisplayName("Debug");
                profile.AddTypeStyle(Types.NumericTypes, Color.DarkMagenta.ToMarkup());
                profile.AddTypeStyle(Types.CharacterTypes, Color.DeepSkyBlue4_2.ToMarkup());
                profile.AddValueStyle(true, Color.DarkGreen.ToMarkup());
                profile.AddValueStyle(false, Color.DarkRed_1.ToMarkup());
                profile.AddTypeStyle<NullValue>(Color.DarkOrange3.ToMarkup());
            });

            options.ConfigureProfile(LogLevel.Information, profile =>
            {
                profile.LogLevel = LogLevel.Information;
                profile.BaseEventStyle = Color.Grey93.ToMarkup();
                profile.SetDefaultTypeStyle(Color.SkyBlue3.ToMarkup());
                profile.SetLogLevelDisplayName("Info");
                profile.AddTypeStyle(Types.NumericTypes, Color.Magenta1.ToMarkup());
                profile.AddTypeStyle(Types.CharacterTypes, Color.SkyBlue3.ToMarkup());
                profile.AddTypeStyle<LogLevel>(profile.BaseEventStyle!);
                profile.AddValueStyle(true, Color.DarkSeaGreen4_1.ToMarkup());
                profile.AddValueStyle(false, Color.Maroon.ToMarkup());
                profile.AddTypeStyle<NullValue>(Color.DarkOrange.ToMarkup());
            });

            options.ConfigureProfile(LogLevel.Warning, profile =>
            {
                profile.LogLevel = LogLevel.Warning;
                profile.BaseEventStyle = Color.LightGoldenrod2_2.ToMarkup();
                profile.SetDefaultTypeStyle(Color.Cyan1.ToMarkup());
                profile.SetLogLevelDisplayName("Warn");
                profile.AddTypeStyle(Types.NumericTypes, Color.Magenta1.ToMarkup());
                profile.AddTypeStyle(Types.CharacterTypes, Color.SkyBlue3.ToMarkup());
                profile.AddTypeStyle<LogLevel>(profile.BaseEventStyle!);
                profile.AddValueStyle(true, Color.DarkSeaGreen4_1.ToMarkup());
                profile.AddValueStyle(false, Color.Maroon.ToMarkup());
                profile.AddTypeStyle<NullValue>(Color.DarkOrange.ToMarkup());
            });

            options.ConfigureProfile(LogLevel.Error, profile =>
            {
                profile.LogLevel = LogLevel.Error;
                profile.BaseEventStyle = Color.Red1.ToMarkup();
                profile.SetDefaultTypeStyle(Color.White.ToMarkup());
                profile.SetLogLevelDisplayName("Error");
                profile.AddTypeStyle<LogLevel>(profile.BaseEventStyle!);
            });

            options.ConfigureProfile(LogLevel.Critical, profile =>
            {
                profile.LogLevel = LogLevel.Error;
                profile.BaseEventStyle = Color.Red1.ToMarkup();
                profile.SetDefaultTypeStyle(Color.White.ToMarkup());
                profile.SetLogLevelDisplayName("Crit");
                profile.ConfigureRenderer<LogLevelRenderer.Options>(opt => opt.Style = "white on red1");
                profile.AddTypeStyle<LogLevel>("white on red1");
            });
            
            options.ConfigureProfiles(profile =>
            {
                profile.ConfigureRenderer<CategoryNameRenderer.Options>(opt => opt.Style = profile.BaseEventStyle!);
                profile.AddTypeFormatter<object>(obj => obj?.ToString());
                
                profile.ConfigureRenderer<ExceptionRenderer.Options>(opt =>
                {
                    opt.ExceptionMessageStyle = Color.Grey93.ToMarkup();
                    opt.ExceptionNameFormatter = type => type.FullName!;
                    opt.ExceptionNameStyle = Color.Grey93.ToMarkup();
                    opt.SourceLineNumberStyle = Color.Magenta1.ToMarkup(); 
                    opt.MaxStackFrames = 5;
                    opt.MethodNameStyle = Color.DarkOrange3_1.ToMarkup();
                    opt.ParameterNameStyle = Color.Grey93.ToMarkup();
                    opt.ParameterTypeStyle = Color.DodgerBlue1.ToMarkup();
                    opt.RenderParameterNames = true;
                    opt.RenderSourceLineNumbers = true;
                    opt.RenderParameterTypes = true;
                    opt.RenderSourcePaths = true;
                    opt.SourcePathFormatter = path => path;
                    opt.SourcePathStyle = Color.Grey50.ToMarkup();
                    opt.StackFrameStyle = Color.Grey50.ToMarkup();
                    opt.UnwindAggregateExceptions = true;
                    opt.UnwindInnerExceptions = true;
                });
            });
        }
    }
}