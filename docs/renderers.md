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

## Out-of-box renderers

|Renderer|Template|Description|
|---|---|---|
|[CategoryNameRenderer](category-name.md)|`{CategoryName}`|Outputs the category name assigned to the logger|
|[EventIdRenderer](event-id.md)|`{EventId}`|Outputs the event id|
|[ExceptionRenderer](exceptions.md)|`{Exception}`|Outputs the exception if set|
|[LogLevelRenderer](log-level.md)|{LogLevel}|Outputs the log level|
|[MarginRenderer](margin-control.md)|{Margin}|Sets the margin position for multi-line output|
|[MessageTemplateRenderer](message-template.md)|{Message}|Outputs the log event message with structured value replacements|
|[NewLineRenderer](new-line.md)|{NewLine}|Outputs a new line|
|[ScopeValueRenderer](scope-values.md)|`{<key>}`|Outputs a structured log or scope value|
|[TimestampRenderer](timestamp.md)|{Timestamp}|Outputs the log event timestamp|
