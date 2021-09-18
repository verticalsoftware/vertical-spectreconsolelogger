# Thread ID Renderer

## Overview

Prints the managed thread id (obtained by `Thread.CurrentThread.ManagedThreadId`).

### Placeholder Syntax

```
{ThreadId}
```

### Value Types

The following type can be formatted & styled (the default formatter prints the `ManagedThreadId`).

| Type                      | Description                     |
| ------------------------- | ------------------------------- |
| `ThreadIdRenderer.Value` | Represents the captured thread. |
