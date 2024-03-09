// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Logging;
using Vertical.SpectreLogger;

var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddSpectreConsole(options =>
    {
        options.SetMinimumLevel(LogLevel.Debug);
        options.SetMinimumLevel("Vertical", LogLevel.Warning);
        
        options.ConfigureProfile(LogLevel.Debug, profile => profile.OutputTemplate =
            "[grey46][[{DateTime:T} Dbug]] {CategoryName}: {Message}{NewLine}{Exception}[/]");
        options.ConfigureProfile(LogLevel.Information, profile => profile.OutputTemplate =
            "[grey85][[{DateTime:T} [green3_1]Info[/]]] {CategoryName}: {Message}{NewLine}{Exception}[/]");
        options.ConfigureProfile(LogLevel.Warning, profile => profile.OutputTemplate =
            "[grey85][[{DateTime:T} [gold1]Warn[/]]] {CategoryName}: {Message}{NewLine}{Exception}[/]");
        options.ConfigureProfile(LogLevel.Error, profile => profile.OutputTemplate =
            "[grey85][[{DateTime:T} [red1]Fail[/]]] {CategoryName}: {Message}{NewLine}{Exception}[/]");
    });
    builder.SetMinimumLevel(LogLevel.Debug);
});

var logger = loggerFactory.CreateLogger("Vertical");

logger.LogDebug("Debug message");
logger.LogInformation("Info message");
logger.LogWarning("Warning message");
logger.LogError("Error message");

var otherLogger = loggerFactory.CreateLogger("Program");
otherLogger.LogDebug("Debug message");
otherLogger.LogInformation("Info message");
otherLogger.LogWarning("Warning message");
otherLogger.LogError("Error message");