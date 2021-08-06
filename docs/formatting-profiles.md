# Formatting profiles

## Overview

Formatting profiles control precisely how events are rendered _for a specific_ log level. This means that events can appear differently depending on their severity. Formatting profiles are configured within logging setup using delegates. Out-of-box, calling `AddSpectreConsole()` will configure default formatting profiles using a basic set of styles and formatting. Any changes made to the profile overwrite the default configuration.

Profiles are configured by setting properties or using extension methods on the `FormattingProfile` type. Configuration settings and customization can be applied to a single profile using `ConfigureProfile`, or to multiple profiles using `ConfigureProfiles`.

```csharp
var loggerFactory = LoggerFactory.Create(builder => 
{
    builder.AddSpectreConsole(options => 
    {
        // Apply configuration to a single event level:
        options.ConfigureProfile(LogLevel.Debug, profile => 
        {
            // configuration
        });    
        
        // Apply configuration to multiple event levels:
        options.ConfigureProfiles(new[]{LogLevel.Trace, LogLevel.Debug}, profile =>
        {
            // configuration
        });
        
        // Apply configuration to all events of all log levels:
        options.ConfigureProfiles(profile =>
        {
            // configuration
        });
    };
});
```

## Property summary

Below is a summary of the properties of the `FormattingProfile` type:

|Property|Description|
|---|---|
|LogLevel|(Read-only) - gets the log level the formatting profile is controlling.|
|OutputTemplate|Controls the structure of each rendered event.|
|BaseEventStyle|The markup to apply before rendering the event.|
|TypeStyles|Controls the markup that is applied to values of specific types.|
|DefaultTypeStyle|Controls the markup that is applied to values when a specific style is not found in `TypeStyles` for a type.|
|TypeFormatters|Functions that provide the string representation of a specific type for rendering.|
|DefaultTypeFormatter|A function that provides a string represents of values for rendering when the type is not found in `TypeFormatters`.
|ValueStyles|Controls the markup that is applied to specific values.|

## Styling overview

> 💡 Note
>
> Any 'Style' property is simply a string with valid Spectre Console markup (e.g. "red1", "white on red1", etc.). It is not necessary to enclose markup in square brackets because the logger implementation does that for you. Furthermore, style tags are automatically closed when their scope ends.
>
> Square brackets that are found in other string properties of options objects are escaped.

Rendering components in the logging implementation use Spectre Console markup strings to decorate the text being rendered. String values found in the various `*Style` properties are enclosed in square brackets and written to the output buffer. The text being decorated is then written to the buffer, and finally the proper closing tag ([/]) is written.

Markup styling can be applied to many distinct parts of the logging event and is detailed in the sections below.

## Property detail

### The output template

The `OutputTemplate` property is a string that controls the structure of each log event. It contains an ordered series of placeholders enclosed in curly braces (handlebars) and other static text. During event rendering, the placeholders are replaced with the output of the specific renderer. Other static text that does not map to a renderer is copied verbatim to the output in the exact order it is found in the string. Here is a simple example:

```csharp
// Configuring the template
profile.OutputTemplate = "[{LogLevel}/{CategoryName}]: {Message}";

// Elsewhere...
logger.LogInformation("Hello Spectre logger");
```

![basic](snips/basic.png)

In addition to several [out-of-box renderers](renderers.md), applications can easily extend the logger with their own [custom renderer](custom-renderer.md) implementations.

### Base event style

The `BaseEventStyle` property is a markup tag that is written before the output template is rendered and closed at the completion of the event. This is the markup the event will fall back to when no other styling is introduced.

