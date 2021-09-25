# Margin Control

## Overview

Sets the margin for subsequent line breaks in the event output.

### Placeholder Syntax

```
{Margin<mode><value>}
```

### Parameters

|Parameter|Description|
|---|---|
|`<mode>`|A control character that indicates the adjustment. `+` increments the current margin value, `-` decrements the current margin value, and `=` sets the absolute value of the margin.|
|`<value>`|The number of spaces to insert before writing characters following a newline break in the event output.|

### Examples

```
{Margin+2}  >> offset current margin by 2 spaces (to the right)
{Margin-2}  >> offset current margin by -2 spaces (to the left)
{Margin=2}  >> set the margin to 2 spaces regardless of where it currently is
```

### Remarks

This renderer does not output content, but more controls future writes to the console. All effects of the renderer apply only to the current log event. The margin is reset when the pipeline is complete.

### Example

You want the logger to behave like Microsoft's console logger. This can be accomplished by customizing the output template and the log level formatting.

```csharp
config.ConfigureProfiles(profile => 
    profile.OutputTemplate = "{LogLevel}: {CategoryName}{Margin=6}{NewLine}{Message}{NewLine+}{Exception}"
);
```

## See Also
- [Next: Message](./message.md)
- [Rendering Overview](./renderer-overview.md)