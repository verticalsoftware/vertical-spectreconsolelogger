# Scope Value Renderer

## Overview

Prints a scope value found matching a specific key in a key/value pair collection.

### Placeholder Syntax

```
{Scope=<key>}
```

### Parameters

|Parameter|Description|
|---|---|
|`<key>`|The key of the scope value to print.|

### Remarks

This renderer prints a scope value by inspecting the scopes available in the log event, determining if any are assignable to `IReadOnlyList<KeyValuePair<string, object>>`, and printing the value of the first entry whose key matches the parameter. If the scope cannot be found, nothing is printed.

> 💡 Note
>
> Scope value placeholders can use alignment, formatting, and destructuring specifiers.

### Example

```csharp
// Given output template: {LogLevel}: userId={Scope=UserId} => {Message}

var userProperty = new KeyValuePair<string,object>("UserId","admin");
var scope = logger.BeginScope(new[]{userProperty});

logger.LogInformation("User connected successfully");

// Output
// Information: userId=admin => User connected successfully
```