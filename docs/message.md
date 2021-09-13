# Message Renderer

## Overview

Prints the message of the log event with applicable structured log value substitutions.

### Placeholder Syntax

```
{Message}
```

### Parameters

None

### Remarks

The message of a log event is string that is given to the `logger.Log()` method. If the message contains structured values, these values are substituted in the output. Log values that are substituted in the message are formatted and styled using the configuration in the applicable log level profile.

> 💡 Note
>
> Log value placeholders can use alignment, formatting, and destructuring specifiers.

### Example

```csharp

// Given the output template: {LogLevel}: The message is \"{Message}\"

logger.LogInformation("Hello from the {provider}!", "vertical spectre logger");

// Output
// Information: The message is "Hello from the vertical spectre logger!"

```