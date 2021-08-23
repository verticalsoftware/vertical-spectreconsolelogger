using System;
using Microsoft.Extensions.Logging;
using Vertical.SpectreLogger.Formatting;

namespace Vertical.SpectreLogger.Rendering
{
    public partial class LogLevelRenderer
    {
        private sealed class Formatter : IFormatter
        {
            internal static readonly IFormatter Default = new Formatter();
            
            /// <inheritdoc />
            public string Format(string format, object value)
            {
                var logLevel = (LogLevel) value;

                return format switch
                {
                    // Serilog formatting
                    "S" => logLevel switch
                    {
                        LogLevel.Trace => "TRC",
                        LogLevel.Debug => "DBG",
                        LogLevel.Information => "INF",
                        LogLevel.Warning => "WRN",
                        LogLevel.Error => "ERR",
                        LogLevel.Critical => "CRT",
                        _ => "NONE"
                    },
                    "V" => logLevel.ToString(),
                    "D" => logLevel switch
                    {
                        LogLevel.Trace => "[Trace]",
                        LogLevel.Debug => "[Debug]",
                        LogLevel.Information => "[Info]",
                        LogLevel.Warning => "[Warn]",
                        LogLevel.Error => "[Error]",
                        LogLevel.Critical => "[Crit]",
                        _ => "[None]"
                    },
                    _ => throw new FormatException($"Invalid format specifier for log level: '{format}'")
                };
            }
        }
    }
}