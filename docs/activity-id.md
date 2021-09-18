# Activity ID Renderer

## Overview

If available, prints the `TraceId` portion of the current `System.Diagnostics.Activity`.

### Placeholder Syntax

```
{ActivityId[,alignment]}
```

### Parameters

|Parameter|Description|
|---|---|
|`[,alignment]`|The preferred formatted field width.|

### Value Types

The following type can be formatted & styled:

|Type|Description|
|---|---|
|`System.Diagnostics.ActivityTraceId`|The [trace id](https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.activity.traceid?view=net-5.0) portion of the activity id.|