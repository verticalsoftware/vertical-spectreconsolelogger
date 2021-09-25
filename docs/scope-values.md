# Scope Values Renderer

## Overview

Prints all scopes available in the log event in sequence order.

### Placeholder Syntax

```
{Scopes}
```

### Parameters

None

### Options Configuration

This renderer defines an `Options` type. The following properties are exposed:

|Property|Description|
|---|---|
|`ContentBefore`|Gets/sets content that is printed before rendering the first scope value.|
|`ContentBetween`|Gets/sets content that is printed between each scope.|
|`ContentAfter`|Gets/sets content that is printed after the last scope value.|

### Remarks

This renderer will print all scope values in the log event in sequence order. If a scope value is a structured log message, it will print the scope with structured log value substitutions.

```csharp
config.ConfigureProfiles(profile => profile
    .ConfigureOptions<ScopeValuesRenderer.Options>(renderer => 
    {
        renderer.ContentBetween = " => ";
        renderer.ContentAfter = " => ";
    }))
    .OutputTemplate = "{LogLevel}: {Scopes}{Message}");
        
// ...

using var connectionScope = logger.BeginScope("connection={connection}, connectionId);
using var userScope = logger.BeginScope("user={userId}", userId);

logger.LogInformation("Session created");

// Output
// Information: connection=103ae4ff => user=tester@vertical.com => Session created 
```

## See Also
- [Next: Thread Id](./thread-id.md)
- [Rendering Overview](./renderer-overview.md)