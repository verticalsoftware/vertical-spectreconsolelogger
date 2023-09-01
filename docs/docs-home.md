# vertical-spectreconsolelogger

A seriously customizable [Spectre Console](https://spectreconsole.net/) provider for Microsoft.Extensions.Logging. **Don't** change how your app logs - change how the logs are presented.

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

## Documentation

- Introductory Topics
  - [Basic Configuration](basic-configuration.md)
  - [Output Templates](output-template.md)
  - [Formatting Log Values](formatting.md)
  - [Styling Log Values](styling.md)
  - [Destructured Output](destructuring.md)
- Output Renderers
  - [Renderers Overview](renderer-overview.md)
  - [ActivityId](activity-id.md)
  - [CategoryName](category-name.md)
  - [DateTime](date-time.md)
  - [Exception](exceptions.md)
  - [MarginControl](margin-control.md)
  - [NewLine](newline.md)
  - [Scope](scope-value.md)
  - [Scopes](scopes-value)
  - [ThreadId](thread-id.md)
- Advanced Topics
  - [Advanced Configuration](advanced-config.md)  
- API
  - Browse assembly documentation on [Tripleslash.io](https://tripleslash.io/docs/.net/vertical-spectreconsolelogger/0.10.1-dev.20230712.19/api/@index?view=net7.0)
  

## Examples

Checkout and run our [examples](https://github.com/verticalsoftware/vertical-spectreconsolelogger/tree/dev/examples) to see the logger in action.
