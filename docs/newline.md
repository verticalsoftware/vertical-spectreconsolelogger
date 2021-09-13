# New Line Renderer

## Overview

Inserts or enqueues a line break in event output.

### Placeholder Syntax

```
{NewLine[+]}
```

### Parameters

|Parameter|Description|
|---|---|
|`+`|Enqueues the new line, such that it is written to the output before any other non new-line character is written. If no other characters are written in the current log event, the new line is ignored.|

### Remarks

Use this renderer to control event output that spans multiple lines. For instance, you may want exceptions to be written on a new line if they are present in the log event.

### Example

```
info/{CategoryName}: {Message}{NewLine+}{Exception}
```

