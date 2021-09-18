# Exception Renderer

## Overview

Prints an exception to the console if available in the log event.

### Placeholder Syntax

```
{Exception[+]}
```

### Parameters

| Parameter | Description                                                  |
| --------- | ------------------------------------------------------------ |
| `[+]`     | Inserts a newline character before the exception is printed. |

### Value Types

The following types can be formatted & styled:

| Type                                      | Description                                                                                        |
| ----------------------------------------- | -------------------------------------------------------------------------------------------------- |
| `ExceptionRenderer.ExceptionNameValue`    | Wraps the type of exception that occurred.                                                         |
| `ExceptionRenderer.ExceptionMessageValue` | Wraps the value of the exception's `Message` property.                                             |
| `ExceptionRenderer.MethodNameValue`       | Wraps the full name of a method in a stack frame.                                                  |
| `ExceptionRenderer.ParameterTypeValue`    | Wraps the type of a parameter in a stack frame method.                                             |
| `ExceptionRenderer.ParameterNameValue`    | Wraps the name of a parameter in a stack frame method.                                             |
| `ExceptionRenderer.SourceDirectoryValue`  | If the source file is available in the stack frame, the directory name of the file.                |
| `ExceptionRenderer.SourceFileValue`       | If the source file is available in the stack frame, the name of the file with extension.           |
| `ExceptionRenderer.SourceLocationValue`   | If the source file is available in the stack frame, the line number where the exception as thrown. |

### Options Configuration

This renderer defines an `Options` type. The following properties are exposed:

| Property                    | Description                                                                                                                                                               |
| --------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `UnwindAggregateExceptions` | Gets/sets whether to display the child exceptions of an `AggregateException`.                                                                                             |
| `UnwindInnerExceptions`     | Gets/sets whether to recursively display inner exceptions.                                                                                                                |
| `MaxStackFrames`            | Gets/sets the maximum number of stack frames to render. If this value is less than the number of available stack frames, the renderer will indicate how many were hidden. |
| `StackFrameIndent`          | Gets/sets the number of indent spaces for the stack frame methods.                                                                                                        |
| `ShowParameterTypes`        | Gets/sets whether to show parameter types in a stack frame method.                                                                                                        |
| `ShowParameterNames`        | Gets/sets whether to show parameter names in a stack frame method.                                                                                                        |
| `ShowSourcePaths`           | Gets/sets whether to show file paths that contain the stack frame method.                                                                                                 |
| `ShowSourceLocations`       | Gets/sets whether to show line numbers after stack frame file paths.                                                                                                      |

Example:

```csharp
profile.ConfigureProfiles(profile =>
    {
        profile.ConfigureOptions<ExceptionRenderer.Options>(renderer =>
            {
                renderer.MaxStackFrames = 5;
                renderer.StackFrameIndent = 3;
                // ...
            });

        // Show line numbers in aqua
        profile.AddTypeStyle<ExceptionRenderer.SourceLocationValue>("[aqua]");

        // Don't print the full directory, print the file name only
        profile.AddTypeFormatter<ExceptionRenderer.SourceDirectoryValue>(
            (format, obj, provider) => string.Empty));
    });
```
