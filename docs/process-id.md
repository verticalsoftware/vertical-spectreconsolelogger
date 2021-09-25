# Process ID Renderer

## Overview

Prints the process id (obtained by `Environment.ProcessId`).

### Placeholder Syntax

```
{ProcessId[,alignment]}
```

### Parameters

|Parameter|Description|
|---|---|
|`[,alignment]`|The preferred formatted field width.|


### Emitted Types

The following type can be formatted & styled (the default formatter prints the `Id`).

| Type                      | Description                     |
| ------------------------- | ------------------------------- |
| `ProcessIdRenderer.Value` | Represents the captured thread. |

## See Also
- [Next: Single Scope Value](./scope-values.md)
- [Rendering Overview](./renderer-overview.md)