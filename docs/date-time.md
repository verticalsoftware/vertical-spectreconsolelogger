# DateTime Renderer

## Overview

Prints the value of `DateTimeOffset.Now`, or the value returned by a factory registered in this type's options.

### Placeholder Syntax

```
{DateTime[,alignment][:format-string]}
```

### Example

```
{DateTime:yyyy/MM/dd hh:mm:ss}
```

### Parameters

|Parameter|Description|
|---|---|
|`[,alignment]`|The preferred formatted field width.|
|`[:format-string]`|A format string that can control how the date is printed (see [DateTime Formatting](https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-date-and-time-format-strings)).|

### Options configuration

This renderer defines an `Options` type. You can customize the date/time value returned by assigning a factory. For example:

```csharp
// Use DateTimeOffset.UtcNow instead of local time:

config.ConfigureProfiles(profile =>
{
    profile.ConfigureOptions<DateTimeRenderer.Options>(renderer =>
        renderer.ValueFactory = () => DateTimeOffset.UtcNow);
});
```

### Emitted Types

The following type(s) can be formatted & styled:

|Type|Description|
|---|---|
|`System.DateTimeOffset`|Represents the category name|

## See Also
- [Next: Exceptions](./exceptions.md)
- [Rendering Overview](./renderer-overview.md)