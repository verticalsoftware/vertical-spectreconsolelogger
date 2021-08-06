# Main Options

## Overview

Configuring the spectre console logger's main options is done within the delegate specified by the `AddSpectreConsoleLogger()` method. The `SpectreLoggerOptions` object provides access to the various top level settings as well as to the log level [formatting profiles](./formatting-profiles.md).

The following properties are available on the `SpectreLoggingOptions` object:

|Property|Description|
|---|---|
|Filters|A dictionary of filters that determine what log events are rendered (see [filters](#Filters) topic below).|
|FormattingProfiles|A read-only dictionary of the six formatting profiles (one for each log level). Using the `ConfigureProfile` or `ConfigureProfiles` methods is recommended instead of accessing the values directly. Refer to the [Formatting Profiles](formatting-profiles.md) documentation for all customization options.
|MinimumLogLevel|The minimum severity level a log event must be to be rendered|

## Filters

Filters can prevent certain events from being rendered by the logger. Unlike `Microsoft.Extensions.Logging` filters, implementations can filter on anything in the event. You can configure filters using any of the methods in the following example:

```csharp
var loggerFactory = LoggerFactory.Create(builder => 
{
    builder.AddSpectreConsole(options => 
    {
        // Filter events below warnings for categories that start with Microsoft.Extensions.Hosting
        options.AddFilter("Microsoft.Extensions.Hosting", LogLevel.Warning);
        
        // Filter events below warnings for only Microsoft.Extensions.Hosting (uses regular expression syntax).
        options.AddFilter("Microsoft.Extensions.Hosting$", LogLevel.Warning);
        
        // Filter events by event id
        options.AddFilter(new EventId(10));
        
        // Filter events using a function
        options.AddFilter((in LogEventInfo e) => 
        {
            // Using data provided in the LogEventInfo, return true to filter the event
            // or false to render it
        });
    };
});
```

## MinimumLogLevel

This value is the top-level control mechanism for determining which log events are rendered. Any events below this level are ignored by the logger. 

## Formatting Profiles

Customizations to colors, formatting, styling, output structure, etc., are grouped together by log level. This means that the way the logger renders events at the `Information` level can be different from the way it renders `Error` events. This provides console applications with maximum flexibility. Refer to the [Formatting Profiles](formatting-profiles.md) documentation for all customization options.