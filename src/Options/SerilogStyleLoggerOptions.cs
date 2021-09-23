using System;
using Microsoft.Extensions.Logging;
using Vertical.SpectreLogger.Destructuring;
using Vertical.SpectreLogger.Formatting;
using Vertical.SpectreLogger.Rendering;

namespace Vertical.SpectreLogger.Options
{
    public static class SerilogStyleLoggerOptions
    {
        public static SpectreLoggingBuilder UseSerilogConsoleStyle(this SpectreLoggingBuilder config)
        {
            const string baseColor = "[grey85]";
            
            config.ConfigureProfiles(profile =>
            {
                profile.TypeStyles.Clear();
                profile.ValueStyles.Clear();

                profile
                    .AddTypeFormatter<LogLevel>((_, value, _) =>
                        value switch
                        {
                            LogLevel.Trace => "VRB",
                            LogLevel.Debug => "DBG",
                            LogLevel.Information => "INF",
                            LogLevel.Warning => "WRN",
                            LogLevel.Error => "ERR",
                            LogLevel.Critical => "CRT",
                            _ => string.Empty
                        })
                    .AddTypeFormatter<NullValue>((_, _, _) => "null")
                    .AddTypeStyle(Types.Numerics, "[magenta3]")
                    .AddTypeStyle(Types.Temporal, "[green]")
                    .AddTypeStyle<Guid>("[green]")
                    .AddTypeStyle<bool>("[royalblue1]")
                    .AddTypeStyle<NullValue>("[royalblue1]")
                    .AddTypeStyle<char>("[green]")
                    .AddTypeStyle<ExceptionRenderer.ExceptionMessageValue>(baseColor)
                    .AddTypeStyle<ExceptionRenderer.ExceptionNameValue>(baseColor)
                    .AddTypeStyle<ExceptionRenderer.MethodNameValue>(baseColor)
                    .AddTypeStyle<ExceptionRenderer.ParameterNameValue>(baseColor)
                    .AddTypeStyle<ExceptionRenderer.ParameterTypeValue>(baseColor)
                    .AddTypeStyle<ExceptionRenderer.SourceDirectoryValue>(baseColor)
                    .AddTypeStyle<ExceptionRenderer.SourceFileValue>(baseColor)
                    .AddTypeStyle<ExceptionRenderer.SourceLocationValue>(baseColor)
                    .AddTypeStyle<LogLevel>(baseColor)
                    .AddTypeStyle<DestructuredKeyValue>(baseColor)
                    .AddTypeStyle<DateTimeRenderer.Value>(baseColor);
                
                profile.OutputTemplate = "[[{DateTime:HH:mm:ss} {LogLevel}]] {Message}{NewLine}{Exception}";
                profile.DefaultLogValueStyle = "[aqua]";
            });
            
            config.ConfigureProfile(LogLevel.Warning, profile => profile.AddTypeStyle<LogLevel>("[wheat1]"));
            config.ConfigureProfile(LogLevel.Error, profile => profile.AddTypeStyle<LogLevel>("[white on red1]"));
            config.ConfigureProfile(LogLevel.Critical, profile => profile.AddTypeStyle<LogLevel>("[white on red1]"));

            return config;
        }
    }
}