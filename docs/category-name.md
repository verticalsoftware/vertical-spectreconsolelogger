# Category Name Renderer

## Overview

Prints the category name of the logger.

### Placeholder syntax

```
{CategoryName[,alignment][:format-string]}
```

### Example

The following example would print the category name using compact-class notation:

```
{CategoryName:C}
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
|`C`|Prints the class name part of the category name (e.g.`Logger` -> `Logger`, `Microsoft.Extensions.Logging.ILogger` -> `ILogger`).|
|`S<count>`|Prints a subset of the category name by splitting it into segments between the dot (.) characters and printing only the last number of segments indicated by a numeric specifier (e.g. for category `Microsoft.Extensions.Logging.Ilogger`, `S1` -> `Ilogger`, `S2` -> `Logging.Ilogger`, etc.) 

### Value Types

The following type(s) can be formatted & styled:

|Type|Description|
|---|---|
|`System.Diagnostics.ActivityTraceId`|Represents the category name|