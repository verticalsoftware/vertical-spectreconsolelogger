using Microsoft.Extensions.Logging;
using Vertical.SpectreLogger;

namespace CustomRenderer
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = LoggerFactory.Create(builder =>
            {
                builder.AddSpectreConsole(config =>
                {
                    config.AddTemplateRenderers();

                    const string template = "[grey85][[{DateTime:T} [green3_1]Info[/]]] (logId={IncrementingId}) {Message}{Exception+}[/]";
                    
                    config.ConfigureProfiles(profile => profile.OutputTemplate = template);
                });
            });

            var logger = factory.CreateLogger<Program>();

            for (var c = 0; c < 5; c++)
            {
                logger.LogInformation("Operation successful");
            }
        }
    }
}
