# vertical-spectreconsolelogger - Formatting profiles

## Overview

All styling, colors, formatting functions, and anything else you can customize is done using a formatting profile. Every log level has a formatting profile which means you can render log event data differently depending on the severity of the event. Even the composite output format can be different for each log level.

Applying settings to formatting profiles can be done several ways depending on the scope of the customization you wish to change. Profile are configured using the `ConfigureProfile` or `ConfigureProfiles` methods on the `SpectreConsoleOptions` object.

```csharp
// Assume 'builder' is an ILoggingBuilder instance
builder => builder.AddSpectreConsole(options =>
{
    // Single profile configuration
    options.ConfigureProfile(LogLevel.Debug, profile =>
    {
        // TODO: Apply settings to debug events
    });

    // Multiple profile configuration
    options.ConfigureProfiles(new[]{LogLevel.Debug, LogLevel.Trace}, profile =>
    {
        // TODO: Apply settings to debug and trace events
    });

    // Apply configuration to all profiles
    options.ConfigureProfiles(profile =>
    {
        // TODO: Apply settings to all events
    });
});
```

## Formatting profile properties

The following properties can be configured on a formatting profile:

> 💡 Note:
>
> Markup strings in formatting profiles do not have to be enclosed in square brackets. The renderers will enclose them for you.

| Property             | Description                                                                                                                                                                                                                                                                                 |
| -------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| OutputTemplate       | String that defines a template by which each log event is rendered. The template contains handlebar expressions that determine the placement of things such as the log level, message, scope values, etc. See the [output-template](output-template.md) documentation for more information. |
| BaseMarkup           | A markup string that is written before any other content in the log event is rendered.                                                                                                                                                                                                      |
| LogLevelMarkup       | A markup string that is written before rendering the log level, and closed after the log level has been rendered. This setting will only be applied if LogLevel renderer is placed in the output template.                                                                                  |
| LogLevelDisplay      | The value that is displayed for the log level (e.g. 'Trace', 'Debug', etc.)                                                                                                                                                                                                                 |
| TypeStyles           | A dictionary that pairs types with markup strings. The markup is applied before rendering log values replaced in a message template that have the matching `System.Type`.                                                                                                                   |
| DefaultTypeStyle     | A markup string that is applied before rendering a log value, and the log value type is not found in the `TypeStyles` dictionary.                                                                                                                                                           |
| TypeFormatters       | A dictionary that pairs types with formatting functions. The formatting function accepts `object?` and returns the desired string representation of the value. The formatted value is what is rendererd in the log event.                                                                   |
| DefaultTypeFormatter | A function that accepts `object?` and returns the desired string representation of the value. This function is used if the type is not found in the `TypeFormatters` dictionary.                                                                                                            |
| RendererOptions      | A dictionary of options type objects used by renderer objects.                                                                                                                                                                                                                              |
