# Event ID renderer

### Overview

```
Template: {EventId[,width][:format]}
```

Renders the event id assigned to the log event. Formatting and styling options are defined using the `EventIdRenderer.Options` type.

### Options

|Option|Description|
|---|---|
|`[,width]`|Used to align the output within a fixed width. Negative values align the text to the left; positive values align the text to the right.|
|`[:format]`|Determines what sub-part of the event id to render. Can be either `Id` or `Name` to render the event id or name, respectively. If omitted, the `.ToString()` value is used.|



### Example

```csharp
var logger = LoggerFactory.Create(builder => builder.AddSpectreConsole(options =>
    {
        options.ConfigureProfile(LogLevel.Information, profile =>
            profile
                .ConfigureRenderer<EventIdRenderer.Options>(opt => opt.Style = "green")
                .OutputTemplate = "[{EventId:Name}]: {Message}");
    }))
    .CreateLogger("Example");

logger.LogInformation(new EventId(10, "Example"), "Showing the event name only");
```

![output](snips/event-id.png)