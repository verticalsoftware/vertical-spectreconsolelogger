# Basic Configuration

The logging provider will set a reasonable set of defaults out-of-box, so you are not required to configure anything to get up and running. Listed below are some basic tips to help you on your customization journey.

## Configuration Overview

Configuration is divided into two main parts:

1. The main `SpectreLoggerOptions` object that controls various global settings of the logger.
2. A collection of `LogLevelProfile` option objects that control the customization to events of a particular log event severity (e.g. Debug, Info, etc.).

### Global Options

All configuration is done during the setup phase and is accessible when calling the `AddSpectreConsole` extension method on the `ILoggingBuilder` interface. The method accepts a configuration delegate that provides a `SpectreLoggerBuilder` object that can be manipulated and is shown in the following example:

```csharp
var loggerFactory = LoggerFactory
    .Create(buidler => 
        {
            builder.AddSpectreConsole(options =>
                {
                    // TODO: Customize the logger here
                });
        });
```

> 💡 Note
> 
> In all of the example hereafter, assume the `options` object is a `SpectreLoggerBuilder` instance.

### Setting the minimum log event level.

The provider will not display any log events introduced that are of less of a severity than a configured value. By default, the provider sets the minimum level to `LogLevel.Information`.

```csharp
// Trace & debug events are not displayed

options.SetMinimumLevel(LogLevel.Information);
```

### Filtering log events using a service.

If a filtering service is provided, the current log event is evaluated before being displayed. You can provide your own implementation of `ILogEventFilter` to handle this function.

```csharp
public class MyLogEventFilter : ILogEventFilter
{
     public bool Filter(in LogEventContext eventContext)
     {
        if (eventContext.EventId.Id == 100)
        {
            // Don't display
            return true;
        }
        
        // Don't filter
        return false;
     }
}
```

> 💡 Note
> 
> It is recommended that you use these log filtering events to customize output particular to console logging. Otherwise, consider using the default filtering mechanisms available in the Microsoft logging implementation to ensure consistent policies across your application's logging infrastructure.

For more simple scenarios, filtering can be handled by a delegate.

```csharp
options.SetLogEventFilter((in LogEventContext context) => context.EventId.Id == 100);
```

### Configuring log level profiles

Log level profiles allow you to customize output based on the event severity. That means that `Warning` events can have different styles, formatting, and output templates than that of `Debug` events. Settings can be applied to all profiles at one time, or to individual profiles.

Log level profiles are discussed in great detail in specific renderer documentation, but the jist is:

```csharp
// Configure the profile of a single level
options.ConfigureProfile(LogLevel.Information, profile => 
    {
        // TODO: Configure
    });
    
// Configure multiple profiles with the same settings
options.ConfigureProfiles(new[]{LogLevel.Trace, LogLevel.Debug}, profile =>
    {
        // TODO: Configure
    });
    
// Configure all profiles with the same settings
optiosn.ConfigureProfiles(profile =>
    {
        // TODO: Configure
    });
```

> ☑️ Tip
> 
> Configure profiles from the least specific to the most specific. Keep in mind you can overwrite settings made in previous configuration calls.

### Controlling the output thread

By default, the logging provider will render marked up content to the `AnsiConsole` on the calling thread. This means the thread will be blocked for the logging event cycle. In most situations this is acceptable. If you need the rendering cycle to not block the calling thread for performance reasons, enact the background thread mode.

```csharp
// Render events in the background
options.WriteInBackground();
```

### Controlling the number of buffers

The logging provider pools write buffers to try to reduce garbage collection (defaults to 5 buffers). You can decrease or increase this value depending on the needs of your application and how multi-threaded it is. If your application primarily operates on a single thread, a single buffer is all that is needed, while thread-intensive applications may make use of additional buffers efficiently.

```csharp
// Set buffers to 1 for a single-threaded application
options.
```

