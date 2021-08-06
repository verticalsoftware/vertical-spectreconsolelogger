# Template Renderers

## Overview 

Log events are output to Spectre Console using renderers. Renderers are provided with event data, and it is their responsibility to populate the write buffer with markup and text. Multiple renderers compose a pipeline that is executed in order. The order of renderers is taken directly from the structure of the output template.

Consider the following output template:

```
[{LogLevel}]: {Message}{Exception:NewLine?}
```

The logger will construct the following pipeline from the output template:
1. Using the internal `StaticSpanRenderer`, output '['.
2. Using the `LogLevelRenderer`, output the log level.
3. Using the internal `StaticSpanRenderer`, output ']: '.
4. Using the `MessageTemplateRenderer`, output the message with the structured log values substitutions.
5. Using the `ExceptionRenderer`, output the exception (if any) on a new line.

Everytime the logger encounters a log event, it invokes the pipeline in this order. This document explains each renderer.

## Out-of-box renderer summary

|Renderer|Template|Description|
|---|---|---|
|`CategoryNameRenderer`|{CategoryName}|Outputs the category name assigned to the logger|
|`EventIdRenderer`|{EventId}|Outputs the event id|
|`ExceptionRenderer`|{Exception}|Outputs the exception if set|
|`FormattedLogValueRenderer`|{&lt;key&gt;}|Outputs a structured log or scope value|
|`LogLevelRenderer`|{LogLevel}|Outputs the log level|
|`MarginRenderer`|{Margin}|Sets the margin position for multi-line output|
|`MessageTemplateRenderer`|{Message}|Outputs the log event message with structured value replacements|
|`NewLineRenderer`|{NewLine}|Outputs a new line|
|`TimestampRenderer`|{Timestamp}|Outputs the log event timestamp|

> 💡 Note
>
> Renderers and any alignment/formatting options are case-sensitive when parsed within the output template.

## Renderer list

### Category Name

```
Template: {CategoryName[,width][:format]}
```

Renders the category name assigned to the logger when it was created. Typically, this is the full name of a class. Formatting and styling options are defined using the `CategoryNameRenderingOptions` type.

|Option|Description|
|---|---|
|`[,width]`|Used to align the output within a fixed width. Negative values align the text to the left; positive values align the text to the right.|
|`[:format]`|The character `S` followed by the number of segments of the category name to be rendered starting with the right-most part.|