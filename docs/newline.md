# New Line Renderer

## Overview

Inserts or enqueues a line break in event output.

### Placeholder Syntax

```
{NewLine}
```

### Parameters

None

### Remarks

Use this renderer to control event output that spans multiple lines. For instance, you may want exceptions to be written on a new line if they are present in the log event.

### Example

```
info/{CategoryName}: {Message}{NewLine}{Exception}
```

## See Also
- [Next: Process Id](./process-id.md)
- [Rendering Overview](./renderer-overview.md)

