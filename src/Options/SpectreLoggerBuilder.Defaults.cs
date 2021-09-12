using System;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Vertical.SpectreLogger.Formatting;
using Vertical.SpectreLogger.Rendering;

namespace Vertical.SpectreLogger.Options
{
    public partial class SpectreLoggerBuilder
    {
        private void ConfigureDefaults()
        {
            AddTemplateRenderers(typeof(SpectreLoggerBuilder).Assembly);
            SetMinimumLevel(LogLevel.Information);
            WriteInForeground();
            UseConsole(AnsiConsole.Console);

            ConfigureProfile(LogLevel.Trace, profile =>
            {
                profile.OutputTemplate = "[grey35][[{DateTime:T} Trc]] {Message}[/]";
                profile.DefaultLogValueStyle = "[grey46]";
            });
            
            ConfigureProfile(LogLevel.Debug, profile =>
            {
                profile.OutputTemplate = "[grey46][[{DateTime:T} Dbg]] {Message}[/]";
                profile.AddTypeStyle(Types.Numerics, "[mediumorchid1]");
                profile.AddTypeStyle(Types.Characters, "[orange3]");
                profile.AddTypeStyle(Types.Temporal, "[deepskyblue3_1]");
                profile.AddValueStyle(true, "[chartreuse4]");
                profile.AddValueStyle(false, "[darkorange3]");
                profile.AddTypeStyle<NullValue>("[grey30]");
                profile.DefaultLogValueStyle = "[cyan3]";
            });
            
            ConfigureProfile(LogLevel.Information, profile =>
            {
                profile.OutputTemplate = "[grey85][[{DateTime:T} [green3_1]Info[/]]] {Message}[/]";
                profile.AddTypeStyle(Types.Numerics, "[mediumorchid1_1]");
                profile.AddTypeStyle(Types.Characters, "[orange1]");
                profile.AddTypeStyle(Types.Temporal, "[deepskyblue1]");
                profile.AddValueStyle(true, "[chartreuse3_1]");
                profile.AddValueStyle(false, "[darkorange3_1]");
                profile.AddTypeStyle<NullValue>("[grey39]");
                profile.DefaultLogValueStyle = "[cyan1]";
            });
            
            ConfigureProfile(LogLevel.Warning, profile =>
            {
                profile.OutputTemplate = "[grey85][[{DateTime:T} [gold1]Wrn[/]]] {Message}[/]";
                profile.AddTypeStyle(Types.Numerics, "[mediumorchid1_1]");
                profile.AddTypeStyle(Types.Characters, "[orange1]");
                profile.AddTypeStyle(Types.Temporal, "[deepskyblue1]");
                profile.AddValueStyle(true, "[chartreuse3_1]");
                profile.AddValueStyle(false, "[darkorange3_1]");
                profile.AddTypeStyle<NullValue>("[grey39]");
                profile.DefaultLogValueStyle = "[cyan1]";
            });
            
            ConfigureProfile(LogLevel.Error, profile =>
            {
                profile.OutputTemplate = "[grey85][[[red1]{DateTime:T} Err[/]]] {Message}[/]";
                profile.AddTypeStyle(Types.Numerics, "[mediumorchid1_1]");
                profile.AddTypeStyle(Types.Characters, "[orange1]");
                profile.AddTypeStyle(Types.Temporal, "[deepskyblue1]");
                profile.AddValueStyle(true, "[chartreuse3_1]");
                profile.AddValueStyle(false, "[darkorange3_1]");
                profile.AddTypeStyle<NullValue>("[grey39]");
                profile.DefaultLogValueStyle = "[cyan1]";
            });
            
            ConfigureProfile(LogLevel.Critical, profile =>
            {
                profile.OutputTemplate = "[[[red1]{DateTime:T}[/] [white on red1]Crt[/]]] [red3] {Message}[/]";
                profile.DefaultLogValueStyle = "[grey78]";
            });

            ConfigureProfiles(profile =>
            {
                profile.AddTypeFormatter<NullValue>((_, _, _) => "(null)");
                profile.AddTypeFormatter<CategoryNameRenderer.ValueType>(new CategoryNameRenderer.DefaultFormatter());
                profile.ConfigureRenderer<DateTimeRenderer.Options>(opt => opt.ValueFactory = () => DateTimeOffset.Now);
            });
        }
    }
}