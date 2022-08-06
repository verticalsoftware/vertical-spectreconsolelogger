using Microsoft.Extensions.Logging;
using Spectre.Console;
using Vertical.SpectreLogger.Destructuring;
using Vertical.SpectreLogger.Formatting;
using Vertical.SpectreLogger.Rendering;

namespace Vertical.SpectreLogger.Options
{
    public partial class SpectreLoggingBuilder
    {
        private void ConfigureDefaults()
        {
            ConfigureProfile(LogLevel.Trace, profile =>
            {
                var baseColor = Color.Grey35;
                profile.OutputTemplate = "[grey35][[{DateTime:T} Trce]] {Message}{NewLine}{Exception}[/]";
                profile.DefaultLogValueStyle = $"[{Color.Grey46}]";
                profile
                    .AddTypeStyle<ExceptionRenderer.MethodNameValue>(baseColor)
                    .AddTypeStyle<ExceptionRenderer.ParameterTypeValue>(baseColor)
                    .AddTypeStyle<ExceptionRenderer.SourceLocationValue>(Color.DarkViolet)
                    .AddTypeStyle<ExceptionRenderer.SourceFileValue>(Color.DarkGoldenrod)
                    .AddTypeStyle<ExceptionRenderer.TextValue>(baseColor)
                    .AddTypeStyle<CategoryNameRenderer.Value>(baseColor)
                    .AddTypeStyle<DateTimeRenderer.Value>(baseColor);
            });
            
            ConfigureProfile(LogLevel.Debug, profile =>
            {
                profile.OutputTemplate = "[grey46][[{DateTime:T} Dbug]] {Message}{NewLine}{Exception}[/]";
                profile
                    .AddTypeStyle(Types.Numerics, Color.DarkViolet)
                    .AddTypeStyle(Types.Characters, Color.DarkOrange3)
                    .AddTypeStyle(Types.Temporal, Color.SlateBlue3)
                    .AddTypeStyle<ExceptionRenderer.ExceptionNameValue>(Color.Grey58)
                    .AddTypeStyle<ExceptionRenderer.ExceptionMessageValue>(Color.DarkOrange3)
                    .AddTypeStyle<ExceptionRenderer.MethodNameValue>(Color.Grey35)
                    .AddTypeStyle<ExceptionRenderer.ParameterTypeValue>(Color.Grey35)
                    .AddTypeStyle<ExceptionRenderer.ParameterNameValue>(Color.MediumPurple4)
                    .AddTypeStyle<ExceptionRenderer.SourceLocationValue>(Color.DarkViolet)
                    .AddTypeStyle<ExceptionRenderer.SourceDirectoryValue>(Color.Grey66)
                    .AddTypeStyle<ExceptionRenderer.SourceFileValue>(Color.DarkGoldenrod)
                    .AddTypeStyle<ExceptionRenderer.TextValue>(Color.Grey35)
                    .AddTypeStyle<DestructuredKeyValue>(Color.Grey70)
                    .AddTypeStyle<CategoryNameRenderer.Value>(Color.Grey46)
                    .AddTypeStyle<DateTimeRenderer.Value>(Color.Grey46)
                    .AddValueStyle(true, Color.DarkSeaGreen4)
                    .AddValueStyle(false, Color.DarkRed_1)
                    .DefaultLogValueStyle = $"[{Color.SlateBlue3_1}]";
            });
            
            ConfigureProfile(LogLevel.Information, profile =>
            {
                profile.OutputTemplate = "[grey85][[{DateTime:T} [green3_1]Info[/]]] {Message}{NewLine}{Exception}[/]";
                profile
                    .AddTypeStyle(Types.Numerics, Color.Magenta3_2)
                    .AddTypeStyle(Types.Characters, Color.Gold3_1)
                    .AddTypeStyle(Types.Temporal, Color.SteelBlue3)
                    .AddTypeStyle<ExceptionRenderer.ExceptionNameValue>(Color.Grey85)
                    .AddTypeStyle<ExceptionRenderer.ExceptionMessageValue>(Color.DarkOrange3)
                    .AddTypeStyle<ExceptionRenderer.MethodNameValue>(Color.Grey42)
                    .AddTypeStyle<ExceptionRenderer.ParameterTypeValue>(Color.Grey42)
                    .AddTypeStyle<ExceptionRenderer.ParameterNameValue>(Color.SlateBlue3)
                    .AddTypeStyle<ExceptionRenderer.SourceLocationValue>(Color.Magenta3_2)
                    .AddTypeStyle<ExceptionRenderer.SourceDirectoryValue>(Color.Grey66)
                    .AddTypeStyle<ExceptionRenderer.SourceFileValue>(Color.Gold3_1)
                    .AddTypeStyle<ExceptionRenderer.TextValue>(Color.Grey42)
                    .AddTypeStyle<DestructuredKeyValue>(Color.Grey70)
                    .AddTypeStyle<CategoryNameRenderer.Value>(Color.Grey85)
                    .AddTypeStyle<DateTimeRenderer.Value>(Color.Grey)
                    .AddValueStyle(true, Color.Green3_1)
                    .AddValueStyle(false, Color.DarkOrange3_1)
                    .DefaultLogValueStyle = $"[{Color.SkyBlue3}]";
            });
            
            ConfigureProfile(LogLevel.Warning, profile =>
            {
                profile.OutputTemplate = "[grey85][[{DateTime:T} [gold1]Warn[/]]] {Message}{NewLine}{Exception}[/]";
                profile
                    .AddTypeStyle(Types.Numerics, Color.Magenta3_2)
                    .AddTypeStyle(Types.Characters, Color.Gold3_1)
                    .AddTypeStyle(Types.Temporal, Color.SteelBlue3)
                    .AddTypeStyle<ExceptionRenderer.ExceptionNameValue>(Color.Grey85)
                    .AddTypeStyle<ExceptionRenderer.ExceptionMessageValue>(Color.DarkOrange3)
                    .AddTypeStyle<ExceptionRenderer.MethodNameValue>(Color.Grey42)
                    .AddTypeStyle<ExceptionRenderer.ParameterTypeValue>(Color.Grey42)
                    .AddTypeStyle<ExceptionRenderer.ParameterNameValue>(Color.SlateBlue3)
                    .AddTypeStyle<ExceptionRenderer.SourceLocationValue>(Color.Magenta3_2)
                    .AddTypeStyle<ExceptionRenderer.SourceDirectoryValue>(Color.Grey66)
                    .AddTypeStyle<ExceptionRenderer.SourceFileValue>(Color.Gold3_1)
                    .AddTypeStyle<ExceptionRenderer.TextValue>(Color.Grey42)
                    .AddTypeStyle<DestructuredKeyValue>(Color.Grey70)
                    .AddTypeStyle<CategoryNameRenderer.Value>(Color.Grey85)
                    .AddTypeStyle<DateTimeRenderer.Value>(Color.Grey85)
                    .AddValueStyle(true, Color.Green3_1)
                    .AddValueStyle(false, Color.DarkOrange3_1)
                    .DefaultLogValueStyle = $"[{Color.SkyBlue3}]";
            });
            
            ConfigureProfile(LogLevel.Error, profile =>
            {
                profile.OutputTemplate = "[grey85][[{DateTime:T} [red1]Fail[/]]] {Message}{NewLine}{Exception}[/]";
                profile
                    .AddTypeStyle(Types.Numerics, Color.Magenta3_2)
                    .AddTypeStyle(Types.Characters, Color.Gold3_1)
                    .AddTypeStyle(Types.Temporal, Color.SteelBlue3)
                    .AddTypeStyle<ExceptionRenderer.ExceptionNameValue>(Color.Grey85)
                    .AddTypeStyle<ExceptionRenderer.ExceptionMessageValue>(Color.DarkOrange3)
                    .AddTypeStyle<ExceptionRenderer.MethodNameValue>(Color.Grey42)
                    .AddTypeStyle<ExceptionRenderer.ParameterTypeValue>(Color.Grey42)
                    .AddTypeStyle<ExceptionRenderer.ParameterNameValue>(Color.SlateBlue3)
                    .AddTypeStyle<ExceptionRenderer.SourceLocationValue>(Color.Magenta3_2)
                    .AddTypeStyle<ExceptionRenderer.SourceDirectoryValue>(Color.Grey66)
                    .AddTypeStyle<ExceptionRenderer.SourceFileValue>(Color.Gold3_1)
                    .AddTypeStyle<ExceptionRenderer.TextValue>(Color.Grey42)
                    .AddTypeStyle<DestructuredKeyValue>(Color.Grey70)
                    .AddTypeStyle<CategoryNameRenderer.Value>(Color.Grey85)
                    .AddTypeStyle<DateTimeRenderer.Value>(Color.Grey85)
                    .AddValueStyle(true, Color.Green3_1)
                    .AddValueStyle(false, Color.DarkOrange3_1)
                    .DefaultLogValueStyle = $"[{Color.SkyBlue3}]";
            });
            
            ConfigureProfile(LogLevel.Critical, profile =>
            {
                profile.OutputTemplate = "[[[red1]{DateTime:T}[/] [white on red1]Crit[/]]] [red3] {Message}{NewLine}{Exception}[/]";
                profile
                    .AddTypeStyle(Types.Numerics, Color.Magenta3_2)
                    .AddTypeStyle(Types.Characters, Color.Gold3_1)
                    .AddTypeStyle(Types.Temporal, Color.SteelBlue3)
                    .AddTypeStyle<ExceptionRenderer.ExceptionNameValue>(Color.Grey85)
                    .AddTypeStyle<ExceptionRenderer.ExceptionMessageValue>(Color.DarkOrange3)
                    .AddTypeStyle<ExceptionRenderer.MethodNameValue>(Color.Grey42)
                    .AddTypeStyle<ExceptionRenderer.ParameterTypeValue>(Color.Grey42)
                    .AddTypeStyle<ExceptionRenderer.ParameterNameValue>(Color.SlateBlue3)
                    .AddTypeStyle<ExceptionRenderer.SourceLocationValue>(Color.Magenta3_2)
                    .AddTypeStyle<ExceptionRenderer.SourceDirectoryValue>(Color.Grey66)
                    .AddTypeStyle<ExceptionRenderer.SourceFileValue>(Color.Gold3_1)
                    .AddTypeStyle<ExceptionRenderer.TextValue>(Color.Grey42)
                    .AddTypeStyle<DestructuredKeyValue>(Color.Grey70)
                    .AddTypeStyle<CategoryNameRenderer.Value>(Color.Grey85)
                    .AddTypeStyle<DateTimeRenderer.Value>(Color.Grey85)
                    .AddValueStyle(true, Color.Green3_1)
                    .AddValueStyle(false, Color.DarkOrange3_1)
                    .DefaultLogValueStyle = $"[{Color.SkyBlue3}]";
            });

            ConfigureProfiles(profile =>
            {
                profile.AddTypeFormatters();
            });
        }
    }
}