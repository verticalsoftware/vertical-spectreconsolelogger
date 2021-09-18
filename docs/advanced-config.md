# Advanced Configuration

## Overview

The following sections details some uncommon but useful configuration settings.

### Controlling the output thread

By default, the logging provider will render marked up content to the `AnsiConsole` on the calling thread. This means the thread will be blocked for the logging event cycle. In most situations this is acceptable. If you need the rendering cycle to not block the calling thread for performance reasons, enact the background thread mode.

```csharp
// Render events in the background
config.WriteInBackground();

// Render events in the foreground (default)
config.WriteInForeground();
```

### Controlling the number of buffers

The logging provider pools write buffers to try to reuse string builders (defaults to 5 buffers). You can decrease or increase this value depending on the needs of your application and how multi-threaded it is. If your application primarily operates on a single thread, a single buffer is all that is needed, while thread-intensive applications may make use of additional buffers efficiently.

```csharp
// Set buffers to 1 for a single-threaded application
config.MaxPooledBuffers = 3;
```