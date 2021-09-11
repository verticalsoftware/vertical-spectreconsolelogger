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

1. Decouples styling and formatting from logging input (e.g. don't change your logging, customize how the events are rendered).
2. Define different customizations for *each log level*.
3. Customize the styling and formatting of specific values or specific types of values.
4. Destructure and output complex types in JSON(ish) notation.
5. Customize the rendering completely using output templates.
6. Extend the logger with your own renderers.

## Documentation

- [Basic Configuration](docs/basic-configuration.md)
- [Output Templates](docs/output-template.md)
