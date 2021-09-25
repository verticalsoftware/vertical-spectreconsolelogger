# Thread ID Renderer

## Overview

Prints the managed thread id (obtained by `Thread.CurrentThread.ManagedThreadId`).

### Placeholder Syntax

```
{ThreadId[,alignment]}
```

### Parameters

|Parameter|Description|
|---|---|
|`[,alignment]`|The preferred formatted field width.|


### Emitted Types

The following type can be formatted & styled (the default formatter prints the `ManagedThreadId`).

| Type                      | Description                     |
| ------------------------- | ------------------------------- |
| `ThreadIdRenderer.Value` | Represents the captured thread. |
