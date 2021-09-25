# Renderer Basics

## Overview

Renderers are components that output specific parts of a log event to the Spectre Console. A pipeline is composed by parsing an [output template](./output-template.md) and instantiating renderers in a specific order. When your application calls `Logger.Log(...)`, the pipeline is executed and your event is written to the console. Each renderer has specific capabilities that are outlined by their respective documentation. This topic explains how to digest the documentation for renderers.

### Placeholder Syntax

The placeholder syntax shows the token pattern you need to include in an output template to leverage the renderer. Placeholders use handlebar notation, which is a pattern nestled between curly braces `{}`. Some renderers support alignment and custom formatting, and this is indicated in the placeholder syntax. Parameters in brackets are optional.

### Formatting

If the renderer supports custom formatting, a table of format codes with their specifiers and a description is listed.

### Emitted Types

The logging provider framework associates formatting with specific types, but this can be problematic because most of the values are intrinsic types such as strings and integers which can be ambiguous with other values.
The provider solves this by wrapping these intrinsic values in specific types. Therefore, when you wish to custom format values like the category name (which is a string), you associate your formatter with the wrapper type and not the intrinsic type.

Example: You want to make sure all category names are capitalized, but category names are strings. The `CategoryNameRenderer` will not send a `string` to the formatting system, it will send an instance of the `CatgegoryNameRenderer.Value` type which simply encapsulates the string. Therefore, could implement the requirement as follows:

```csharp
config.ConfigureProfiles(profile =>
{
    profile.AddTypeFormatter<CategoryNameRenderer.Value>((format, obj) =>
    {            
        var categoryName = obj.Value;
        return char.IsLower(categoryName[0])
            ? char.ToUpper(categoryName[0]) + categoryName[1..]
            : categoryName;
    });
});
```

The same principal is also applied to associating markup styles to types.

Example: You have a console application that talks with REST services to manage resources. To indicate whether a call succeeded or failed, you want to display the string "OK" in green and "FAILED" in red. You could introduce two types that represent these values and style them independently.

```csharp
// Define the value types
public sealed class OkMessage
{
    public static readonly OkMessage Value = new OkValue();
    
    private OkMessage() {}
    
    public override string ToString() => "OK";
}

public sealed class FailedMessage
{
    public static readonly FailedMessage Value = new FailedMessage();
    
    private FailedMessage() {}
    
    public override string ToString() => "FAILED";
}

// Configure how they are styled...
config.ConfigureProfiles(profile =>
{
    profile.AddTypeStyle<OkMessage>("[green1]");
    profile.AddTypeStyle<FailedMessage>("[red1]");
});
    
// Then in logging...
logger.LogInformation("{ok} - the record was saved successfully", OkMessage.Value);
```

### Advanced Renderer Options

Some renderers provide advanced capabilities which would be difficult to configure in a format string (for example, the `ExceptionRenderer`). In these cases, the renderer defines an `Options` object that can be configured in the log level profile. They are typically a sub-type of the renderer so they are easy to find. You can configure advanced settings of a renderer by using the `ConfigureOptions` method.

```csharp
// Configure how exceptions are rendered

config.ConfigureProfiles(profile =>
{
    profile.ConfigureOptions<ExceptionRenderer.Options>(renderer =>
        {
            renderer.MaxStackFrames = 5;
            renderer.UnwindAggregateExceptions = true;
            // ...
        });
});
```