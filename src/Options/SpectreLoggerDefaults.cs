using System.Linq;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Vertical.SpectreLogger.PseudoTypes;

namespace Vertical.SpectreLogger.Options
{
    public partial class SpectreLoggerOptions
    {
        /// <summary>
        /// Defines the default output template.
        /// </summary>
        public const string OutputTemplate = "{LogLevel,-5} : {CategoryName} {Margin:+8}{Message}{Exception:NewLine}";

        private static void ConfigureDefaults(SpectreLoggerOptions options)
        {
            options.ConfigureProfile(LogLevel.Trace, profile =>
            {
                profile.LogLevel = LogLevel.Trace;

                profile.BaseEventStyle = Color.Grey35.ToMarkup();
                profile.DefaultTypeStyle = Color.Purple4_1.ToMarkup();
                profile.AddTypeFormatter<LogLevel>(_ => "Trace");
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
                profile.DefaultTypeStyle = Color.Purple4_1.ToMarkup();
                profile.AddTypeFormatter<LogLevel>(_ => "Debug");
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
                profile.DefaultTypeStyle = Color.SkyBlue3.ToMarkup();
                profile.AddTypeFormatter<LogLevel>(_ => "Info");
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
                profile.DefaultTypeStyle = Color.Cyan1.ToMarkup();
                profile.AddTypeFormatter<LogLevel>(_ => "Warn");
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
                profile.DefaultTypeStyle = Color.White.ToMarkup();
                profile.AddTypeFormatter<LogLevel>(_ => "Error");
                profile.AddTypeStyle<LogLevel>(profile.BaseEventStyle!);
            });

            options.ConfigureProfile(LogLevel.Critical, profile =>
            {
                profile.LogLevel = LogLevel.Error;
                profile.BaseEventStyle = Color.Red1.ToMarkup();
                profile.DefaultTypeStyle = Color.White.ToMarkup();
                profile.AddTypeFormatter<LogLevel>(_ => "Crit");
                profile.AddTypeStyle<LogLevel>("white on red1");
            });
            
            options.ConfigureProfiles(profile =>
            {
                profile.ConfigureRenderer<CategoryNameRenderingOptions>(opt => opt.Style = profile.BaseEventStyle!);
                profile.AddTypeFormatter<EventId>(id => id.Id != 0 || !string.IsNullOrWhiteSpace(id.Name)
                    ? id.ToString()
                    : null);
                profile.AddTypeFormatter<object>(obj => obj?.ToString());
                
                profile.ConfigureRenderer<ExceptionRenderingOptions>(opt =>
                {
                    opt.ExceptionMessageMarkup = Color.Grey93.ToMarkup();
                    opt.ExceptionNameFormatter = type => type.FullName!;
                    opt.ExceptionNameMarkup = Color.Grey93.ToMarkup();
                    opt.SourceLineNumberMarkup = Color.Magenta1.ToMarkup(); 
                    opt.MaxStackFrames = 5;
                    opt.MethodNameMarkup = Color.DarkOrange3_1.ToMarkup();
                    opt.ParameterNameMarkup = Color.Grey93.ToMarkup();
                    opt.ParameterTypeMarkup = Color.DodgerBlue1.ToMarkup();
                    opt.SourcePathFormatter = path => path;
                    opt.SourcePathMarkup = Color.Grey50.ToMarkup();
                    opt.StackFrameMarkup = Color.Grey50.ToMarkup();
                    opt.UnwindAggregateExceptions = true;
                });
            });
        }
    }
}