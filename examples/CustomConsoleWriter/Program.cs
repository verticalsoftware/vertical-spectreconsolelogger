// See https://aka.ms/new-console-template for more information

using CustomConsoleWriter;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Vertical.SpectreLogger;
using Vertical.SpectreLogger.Output;

var factory = LoggerFactory.Create(builder =>
{
    builder.AddSpectreConsole();
    builder.Services.AddSingleton<IConsoleWriter, NewLineReplacingWriter>();
});

var logger = factory.CreateLogger<Program>();

logger.LogInformation(
    "This string used to contain a new line character," +
    Environment.NewLine +
    "but now it is removed");
    
    