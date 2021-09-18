# Creating a Custom Renderer

## Overview

You can extend the logging provider functionality by creating your own custom rendering components. Doing so involves creating an implementation of `ITemplateRenderer`. We'll walk through all the steps need to create a custom renderer.

In the following example we'll be building a renderer that generates an incrementing unique id and then build the functionality from the most basic to the most complete.

## Basic Implementation

There are only two requirements for a basic implementation. We need to create a class that implements `ITemplateRenderer`, and we need that class to define the placeholder so it can be matched in the output template.

For this example, let's assume there is a type called `IncrementingId`. It simply encapsulates a 128-bit identitifer that increments for every new value that is instantiated. The snippet below creates a renderer for this type.

```csharp
[Template("{IncrementingId}")]
public class IncrementingIdRenderer : ITemplateRenderer
{
    /// <inheritdoc />
    public void Render(IWriteBuffer buffer, in LogEventContext context)
    {
        buffer.WriteLogValue(
            context.Profile,
            null,
            IncrementingId.Create());
    }
}
```

We'll complete the basic implementation by registering the renderer and changing the output templates to include it.

```csharp
// Adds the template renderers from this assembly
config.AddTemplateRenderers();

// Reference the new renderer in output templates
const string template = "{IncrementingId}/{LogLevel}: {Message}{Exception+}";
config.ConfigureProfiles(profile => profile.OutputTemplate = template);
```

With no other modifications, each log event will now generate and render the value of the incrementing id.

### Supporting Alignment

If you determine that the value printed by the renderer is suitable for output alignment (consumer can specify alignment in the output template, e.g. `{IncrementingId,-24}`), the matching template defined by the renderer must be modified.

Under the hood, the provider builds the rendering pipeline by using regular expressions to match the different segments of the output template. The framework will pattern match the renderer template with the output template. Therefore, what's defined in the template is a regular expression pattern. The logging provider wraps the matches into the `TemplateSegment` type. The reason for this is to provide consistency in naming capture groups and more importantly, rendering values with alignment and formatting consistently.

Let's change how the template is presented by the renderer. Also, we'll ask for the `TemplateSegment` to be injected to our renderer.

```csharp
public class IncrementingIdRenderer : ITemplateRenderer
{
    [Template]
    public static readonly string Template = TemplatePatternBuilder
        .ForKey("IncrementingId")
        .AddAlignment()
        .Build();

    private readonly TemplateSegment _templateSegment;

    public IncrementingIdRenderer(TemplateSegment templateSegment) => _templateSegment = templateSegment;

    /// <inheritdoc />
    public void Render(IWriteBuffer buffer, in LogEventContext context)
    {
        buffer.WriteLogValue(
            context.Profile,
            _templateSegment,
            IncrementingId.Create());
    }
}
```

Here is the summary of the changes made:

1. We moved the `Template` attribute from the class declaration level down to a static property. We did this so we can leverage `TemplatePatternBuilder`, which builds the matching patterns consistently.

2. A constructor was introduced that captures a `TemplateSegment`.

3. The `TemplateSegment` This object wraps the regular expression match and can be passed to `IWriteBuffer` which in turn will perform alignment, formatting and styling automatically.

With these changes in place, we can now specify the alignment in the output template like so:

```
[grey85][[{DateTime:T} [green3_1]Info[/]]] (logId={IncrementingId,-25}) {Message}{Exception+}[/]
```

### Supporting Formatting

In a similar fashion, the renderer can support formatting by simply adding `AddFormatting` to the `TemplatePatternBuilder` fluent chain. In this case, `TemplateSegment` will capture a format specifier, and pass that along when printing the formatted value. This assumes the value type directly supports custom formatting via `IFormattable` or an `ICustomFormatter` is registered.

```csharp
public class IncrementingIdRenderer : ITemplateRenderer
{
    [Template]
    public static readonly string Template = TemplatePatternBuilder
        .ForKey("IncrementingId")
        .AddAlignment()
        .AddFormatting() // <-- Adding formatting support
        .Build();

    // Existing code
}
```

With this change in place, we can now specific formatting in an output template:

```
[grey85][[{DateTime:T} [green3_1]Info[/]]] (logId={IncrementingId,-25:N}) {Message}{Exception+}[/]
```

### Adding a custom control pattern

You can add a pattern that will be placed right after your template key that can either be optional or not and will be captured independently from the key.

```csharp
public class IncrementingIdRenderer : ITemplateRenderer
{
    [Template]
    public static readonly string Template = TemplatePatternBuilder
        .ForKey("IncrementingId")
        // This will now match {IncrementingId} or {IncrementingId*}
        .AddControlPattern("\\*", isOptional: true)
        .AddAlignment()
        .AddFormatting() // <-- Adding formatting support
        .Build();

    private readonly TemplateSegment _templateSegment;

    public IncrementingIdRenderer(TemplateSegment templateSegment) => _templateSegment = templateSegment;

    public void Render(IWriteBuffer buffer, in LogEventContext context)
    {
        var splatSpecified = _templateSegment.ControlCode == "*";

        // Render differently based on splatSpecified
    }
}

```

### Further reading

The best way to learn the full features with examples of `ITemplateRenderer` are to dig into the logging provider source code. The out-of-box renderers can be found in the the `/Rendering` subfolder.

