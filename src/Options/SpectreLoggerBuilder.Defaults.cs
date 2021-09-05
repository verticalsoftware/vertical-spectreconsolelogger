using Spectre.Console;
using Vertical.SpectreLogger.Formatting;

namespace Vertical.SpectreLogger.Options
{
    public partial class SpectreLoggerBuilder
    {
        private void ConfigureDefaults()
        {
            WriteInForeground();
            UseConsole(AnsiConsole.Console);

            ConfigureProfiles(profile =>
            {
                profile.OutputTemplate = "{Message}{Newline+}";
            });

            AddTypeFormatter<NullValue>((_, _, _) => "(null)");
            AddTypeStyle<int>("[magenta2]");
        }
    }
}