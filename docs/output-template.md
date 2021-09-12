# The Output Template

## Overview

The output template controls the content that is displayed for each log event. The output template is a property of a `LogLevelProfile`, so you can customize the output of events specifically for each level.

The output template that is a string that contains Spectre Console markup, static text, and (most importantly) handlebar placeholders that map to specific rendering components. Displayed below is an exmaple output template:

```
[grey85][[{DateTime:T} [green3_1]Info[/]]] {Message}{NewLine+}{Exception}[/]
```
This template renders output events as follows:
1. It sets the [grey85](https://spectreconsole.net/appendix/colors) color using Spectre Console markup.
2. Print an open bracket.
3. Display the current timestamp, formatting it using the `T` code (see [DateTime formatting](https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings))
4. Set the [green3_1](https://spectreconsole.net/appendix/colors) color and display "Info", then close the markup tag.
5. Print a close bracket.
6. Display the event message, performing structured log value substitutions.
7. Queue a new line character that is only printed if an exception is available in the log event.
8. Display the exception if available in the log event.
9. Close the opening markup tag.

The handlebar placeholders in the template string map to specific rendering components in the logging provider. Out-of-box, the following renderers are available:

|Placeholder|Description|
|---|---|
|`{Category}`|Prints the logger category|
|`{DateTime}`|Prints the current date/timestamp|
|`{LogLevel}`|Displays the log level (useful for a single output template)|
|`{Margin}`|Sets the left margin for newline characters. All output is aligned to this margin for the remained of the event (unless changed again)|
|`{Message}`|Display the log message along with structured log value substitutions|
|`{NewLine}`|Prints a newline character for a multi-line log level event|
|`{Scope=<name>}`|Prints the value of a single scope value|
|`{Scopes}`|Prints all scope values|

After configuration but before logging startup, the provider will build an efficient rendering pipeline for each log level. The specific renderers are discussed in their own documentation.

Configure the output template for profiles by setting the `OutputTemplate` property.

```csharp
options.ConfigureProfile(LogLevel.Information, profile => 
    {
        profile.OutputTemplate = "[grey85][[{DateTime:T} [green3_1]Info[/]]] {Message}{NewLine+}{Exception}[/]"));
    });
```