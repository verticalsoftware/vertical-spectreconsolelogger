using Microsoft.Extensions.Logging;
using Vertical.SpectreLogger.Formatting;

namespace Vertical.SpectreLogger.Options
{
    public partial class SpectreLoggerOptions
    {
        public SpectreLoggerOptions()
        {
            const string defaultTemplate = "{LogLevel,-7:D}: {CategoryName:F}{Margin:9}{NewLine}{Scopes}{Message}{NewLine??}{Exception}";
            var options = this;

            this.SetMinimumLogLevel(LogLevel.Trace);

            options.ConfigureProfiles(profile =>
            {
                profile.OutputTemplate = defaultTemplate;
                profile.DefaultLogValueFormatter = new ValueFormatter<object>(value => value.ToString() ?? string.Empty);
                
                profile.ConfigureRenderer<ExceptionRendererOptions>(renderer =>
                {
                    renderer.ExceptionFormatter = ex => $"{ex.GetType()}: {ex.Message}";
                    renderer.MethodFormatter = method => method;
                    renderer.FilePathFormatter = path => path;
                    renderer.MaxStackFrames = 5;
                    renderer.HiddenStackFramesFormatter = hidden => $"(+{hidden} more...)";
                    renderer.MethodParameterFormatter = method => $"{method.type} {method.name}";
                    renderer.StackFrameFormatter = frame => $"  at {frame.method} in {frame.file}:{frame.line}";
                    renderer.FileLineNumberFormatter = line => $"line {line}";
                    renderer.UnwindAggregateExceptions = true;
                    renderer.UnwindInnerExceptions = true;
                });

                profile.ConfigureRenderer<ScopesRendererOptions>(renderer =>
                {
                    renderer.Separator = " => ";
                    renderer.PostRenderFormat = " => ";
                    renderer.RenderNullScopes = false;
                });
            });

            options.ConfigureProfile(LogLevel.Trace, profile =>
            {
            });
            
            options.ConfigureProfile(LogLevel.Debug, profile =>
            {
            });
            
            options.ConfigureProfile(LogLevel.Information, profile =>
            {

            });
            
            options.ConfigureProfile(LogLevel.Warning, profile =>
            {
            });
            
            options.ConfigureProfile(LogLevel.Error, profile =>
            {
            });
            
            options.ConfigureProfile(LogLevel.Critical, profile =>
            {
            });
        }
    }
}