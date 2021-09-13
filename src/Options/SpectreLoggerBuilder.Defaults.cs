using System;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Vertical.SpectreLogger.Destructuring;
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
                profile.OutputTemplate = "[grey35][[{DateTime:T} Trc]] {Message}{NewLine+}{Exception}[/]";
                profile.DefaultLogValueStyle = "[grey46]";
                profile
                    .AddTypeStyle<ExceptionRenderer.MethodNameValue>("[grey35]")
                    .AddTypeStyle<ExceptionRenderer.ParameterTypeValue>("[grey35]")
                    .AddTypeStyle<ExceptionRenderer.SourceLocationValue>("[darkviolet]")
                    .AddTypeStyle<ExceptionRenderer.SourceFileValue>("[darkgoldenrod]");
            });
            
            ConfigureProfile(LogLevel.Debug, profile =>
            {
                profile.OutputTemplate = "[grey46][[{DateTime:T} Dbg]] {Message}{NewLine+}{Exception}[/]";
                profile
                    .AddTypeStyle(Types.Numerics, "[darkviolet]")
                    .AddTypeStyle(Types.Characters, "[darkorange3]")
                    .AddTypeStyle(Types.Temporal, "[slateblue3]")
                    .AddTypeStyle<ExceptionRenderer.ExceptionNameValue>("[grey58]")
                    .AddTypeStyle<ExceptionRenderer.ExceptionMessageValue>("[darkorange3]")
                    .AddTypeStyle<ExceptionRenderer.MethodNameValue>("[grey35]")
                    .AddTypeStyle<ExceptionRenderer.ParameterTypeValue>("[grey35]")
                    .AddTypeStyle<ExceptionRenderer.ParameterNameValue>("[mediumpurple4]")
                    .AddTypeStyle<ExceptionRenderer.SourceLocationValue>("[darkviolet]")
                    .AddTypeStyle<ExceptionRenderer.SourceDirectoryValue>("[grey66]")
                    .AddTypeStyle<ExceptionRenderer.SourceFileValue>("[darkgoldenrod]")
                    .AddTypeStyle<DestructuredKeyValue>("[grey70]")
                    .AddValueStyle(true, "[darkseagreen4]")
                    .AddValueStyle(false, "[darkred_1]")
                    .DefaultLogValueStyle = "[slateblue3_1]";
            });
            
            ConfigureProfile(LogLevel.Information, profile =>
            {
                profile.OutputTemplate = "[grey85][[{DateTime:T} [green3_1]Info[/]]] {Message}{NewLine+}{Exception}[/]";
                profile
                    .AddTypeStyle(Types.Numerics, "[magenta3_2]")
                    .AddTypeStyle(Types.Characters, "[gold3_1]")
                    .AddTypeStyle(Types.Temporal, "[steelblue3]")
                    .AddTypeStyle<ExceptionRenderer.ExceptionNameValue>("[grey85]")
                    .AddTypeStyle<ExceptionRenderer.ExceptionMessageValue>("[gold3_1]")
                    .AddTypeStyle<ExceptionRenderer.MethodNameValue>("[grey42]")
                    .AddTypeStyle<ExceptionRenderer.ParameterTypeValue>("[grey42]")
                    .AddTypeStyle<ExceptionRenderer.ParameterNameValue>("[slateblue3]")
                    .AddTypeStyle<ExceptionRenderer.SourceLocationValue>("[magenta3_2]")
                    .AddTypeStyle<ExceptionRenderer.SourceDirectoryValue>("[grey66]")
                    .AddTypeStyle<ExceptionRenderer.SourceFileValue>("[gold3_1]")
                    .AddTypeStyle<DestructuredKeyValue>("[grey70]")
                    .AddValueStyle(true, "[green3_1]")
                    .AddValueStyle(false, "[darkorange3_1]")
                    .DefaultLogValueStyle = "[skyblue3]";
            });
            
            ConfigureProfile(LogLevel.Warning, profile =>
            {
                profile.OutputTemplate = "[grey85][[{DateTime:T} [gold1]Wrn[/]]] {Message}{NewLine+}{Exception}[/]";
                profile
                    .AddTypeStyle(Types.Numerics, "[magenta3_2]")
                    .AddTypeStyle(Types.Characters, "[gold3_1]")
                    .AddTypeStyle(Types.Temporal, "[steelblue3]")
                    .AddTypeStyle<ExceptionRenderer.ExceptionNameValue>("[grey85]")
                    .AddTypeStyle<ExceptionRenderer.ExceptionMessageValue>("[gold3_1]")
                    .AddTypeStyle<ExceptionRenderer.MethodNameValue>("[grey42]")
                    .AddTypeStyle<ExceptionRenderer.ParameterTypeValue>("[grey42]")
                    .AddTypeStyle<ExceptionRenderer.ParameterNameValue>("[slateblue3]")
                    .AddTypeStyle<ExceptionRenderer.SourceLocationValue>("[magenta3_2]")
                    .AddTypeStyle<ExceptionRenderer.SourceDirectoryValue>("[grey66]")
                    .AddTypeStyle<ExceptionRenderer.SourceFileValue>("[gold3_1]")
                    .AddTypeStyle<DestructuredKeyValue>("[grey70]")
                    .AddValueStyle(true, "[green3_1]")
                    .AddValueStyle(false, "[darkorange3_1]")
                    .DefaultLogValueStyle = "[skyblue3]";
            });
            
            ConfigureProfile(LogLevel.Error, profile =>
            {
                profile.OutputTemplate = "[grey85][[{DateTime:T} [red1]Err[/]]] {Message}{NewLine+}{Exception}[/]";
                profile
                    .AddTypeStyle(Types.Numerics, "[magenta3_2]")
                    .AddTypeStyle(Types.Characters, "[gold3_1]")
                    .AddTypeStyle(Types.Temporal, "[steelblue3]")
                    .AddTypeStyle<ExceptionRenderer.ExceptionNameValue>("[grey85]")
                    .AddTypeStyle<ExceptionRenderer.ExceptionMessageValue>("[gold3_1]")
                    .AddTypeStyle<ExceptionRenderer.MethodNameValue>("[grey42]")
                    .AddTypeStyle<ExceptionRenderer.ParameterTypeValue>("[grey42]")
                    .AddTypeStyle<ExceptionRenderer.ParameterNameValue>("[slateblue3]")
                    .AddTypeStyle<ExceptionRenderer.SourceLocationValue>("[magenta3_2]")
                    .AddTypeStyle<ExceptionRenderer.SourceDirectoryValue>("[grey66]")
                    .AddTypeStyle<ExceptionRenderer.SourceFileValue>("[gold3_1]")
                    .AddTypeStyle<DestructuredKeyValue>("[grey70]")
                    .AddValueStyle(true, "[green3_1]")
                    .AddValueStyle(false, "[darkorange3_1]")
                    .DefaultLogValueStyle = "[skyblue3]";
            });
            
            ConfigureProfile(LogLevel.Critical, profile =>
            {
                profile.OutputTemplate = "[[[red1]{DateTime:T}[/] [white on red1]Crt[/]]] [red3] {Message}{NewLine+}{Exception}[/]";
                profile
                    .AddTypeStyle(Types.Numerics, "[magenta3_2]")
                    .AddTypeStyle(Types.Characters, "[gold3_1]")
                    .AddTypeStyle(Types.Temporal, "[steelblue3]")
                    .AddTypeStyle<ExceptionRenderer.ExceptionNameValue>("[grey85]")
                    .AddTypeStyle<ExceptionRenderer.ExceptionMessageValue>("[gold3_1]")
                    .AddTypeStyle<ExceptionRenderer.MethodNameValue>("[grey42]")
                    .AddTypeStyle<ExceptionRenderer.ParameterTypeValue>("[grey42]")
                    .AddTypeStyle<ExceptionRenderer.ParameterNameValue>("[slateblue3]")
                    .AddTypeStyle<ExceptionRenderer.SourceLocationValue>("[magenta3_2]")
                    .AddTypeStyle<ExceptionRenderer.SourceDirectoryValue>("[grey66]")
                    .AddTypeStyle<ExceptionRenderer.SourceFileValue>("[gold3_1]")
                    .AddTypeStyle<DestructuredKeyValue>("[grey70]")
                    .AddValueStyle(true, "[green3_1]")
                    .AddValueStyle(false, "[darkorange3_1]")
                    .DefaultLogValueStyle = "[skyblue3]";
            });

            ConfigureProfiles(profile =>
            {
                profile.AddTypeFormatters();
                profile.ConfigureOptions<DateTimeRenderer.Options>(opt => opt.ValueFactory = () => DateTimeOffset.Now);
            });
        }
    }
}