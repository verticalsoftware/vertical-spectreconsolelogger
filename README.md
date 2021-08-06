# vertical-spectreconsolelogger

A seriously customizable [Spectre Console](https://spectreconsole.net/) provider for Microsoft.Extensions.Logging.

## Quick Start

Add a package reference to your `.csproj` file:

```
$ dotnet add package vertical-spectreconsolelogger --prerelease
```

Call `AddSpectreConsole` in your logging setup:

```csharp
var loggerFactory = LoggerFactory.Create(builder => builder
    .AddSpectreConsole());

var logger = loggerFactory.CreateLogger("MyLogger");

logger.LogInformation("Hello world!");
```

## Features at a glance

1. Customize the content of values being rendered using formatting functions.
2. Customize how specific values or values of specific types are styled and decorated.
3. Structure the output of log events using message templates.
4. Customize at the granular level of detail how exceptions are rendered (limit stack frames, shorten file paths, shorten class names, etc.)
5. Apply customizations to events of different log levels using formatting profiles.
6. Extend the logger implementation with custom renderers.

## Documentation

[Configuring main options](docs/main-options.md)