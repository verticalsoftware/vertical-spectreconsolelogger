# Category Name Renderer

## Overview

Prints the category name of the logger.

### Placeholder syntax

```
{CategoryName[,alignment][:format-string]}
```

### Parameters

|Parameter|Description|
|---|---|
|`[,alignment]`|The preferred formatted field width.|
|`[:format-string]`|A format string that can control how the category name is printed.|

### Formatting

The format string for this renderer currently supports two notations:

|Format|Description|
|---|---|
|`C`|Prints the class name only, or the last segment of the logger name assuming it can be split by the `.` character. Example: `Microsoft.Extensions.Logging.ILogger` would print `ILogger`.
|`C<d>`|Where `d` is the number of segments in the class name to print, beginning at the end of the name. Example: `C2` applied to `Microsoft.Extensions.Logging.ILogger` would print `Logging.ILogger`.

> 💡 Note
> 
> You can assume complete control of formatting by registering a formatter for the `CategoryName` value wrapper type (see [Rendering Overview](./renderer-overview.md)).