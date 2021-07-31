# vertical-spectreconsolelogger

A seriously customizable [Spectre Console](https://spectreconsole.net/) provider for Microsoft.Extensions.Logging.

## Quick Start

Call `AddSpectreConsole` in your logging setup:

```csharp
var loggerFactory = LoggerFactory.Create(builder => builder
    .AddSpectreConsole());

var logger = loggerFactory.CreateLogger("MyLogger");

logger.LogInformation("Hello world!");
```

## Features at a glance

1. Customize the color and style of log values on a per-type basis.
2. Define formatting functions that control how log values are presented.
3. Fine-grain control on exception rendering (max stack frames, source path formatting, method formatting, `AggregateException` unwinds, etc.).
4. Control composition with output templates.
5. Apply different formatting, style, and color profiles to each log level.
6. Extend the templating system with custom renderers.

## Documentation

- [Formatting profiles](docs/formatting-profiles)
- [The output template](docs/something)
- [Exceptions](docs/something)
- [Scopes](docs/something)
- [Creating your own renderer](docs/something)
