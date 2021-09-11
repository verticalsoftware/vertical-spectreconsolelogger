# Renderer Overview

## Overview

Renderer components print things to the console. They pull data from the `LogEventContext` structure and print whatever their specific thing is to print to the console. Renderers are mapped from the [output template](./output-template.md) and composed into an ordered pipeline for each log level profile.

The handlebar notation for each renderer can be a simple token, but often customizations to how the renderer formats its output can be specified. Some renderers support .Net composite formatting, and therefore their field width and formatting can be controlled using a familiar syntax akin to `String.Format` semantics.

Consider the `CategoryNameRenderer`. It supports width and output formatting, so while you can include `{CategoryName}` in the output template, the renderer also supports field width and composite formatting like so:

```
{CategoryName,-25:C}
```

Here, the category name will occupy a minimum of 25 character spaces, and be formatted in 'Compact' notation (just the class name).

Each renderer has verbose documentation for the formatting it supports, so be sure to consult each one when the out-of-box behavior does not suit your needs.

If a renderer supports custom formatting, you can override the formatting behavior by registering a custom formatter object.

For instance, consider you want to print the category name in the log event, but you want to trim long class names down. You can define your own formatter which the provider will use anytime it prints a category name:

```csharp
public class CategoryNameFormatter : ICustomFormatter
{
    /// <inheritdoc />
    public string Format(string? format, object? arg, IFormatProvider? formatProvider)
    {
        if (arg == null)
        {
            return string.Empty;
        }

        if (format == null)
        {
            return arg.ToString();
        }

        var match = Regex.Match(format, @"T(\d)");

        if (!match.Success)
        {
            return arg.ToString();
        }

        var maxParts = int.Parse(match.Groups[1].Value);
        var categoryName = ((CategoryName) arg).Value;
        var split = categoryName.Split('.');

        return split.Length <= maxParts
            ? categoryName
            : string.Join('.', split.Reverse().Take(maxParts).Reverse());
    }
}
```

Then, register this formatter the log level profiles using the `CategoryName` pseudo-type (pseudo-types are types provided by the provider that are really just strings, but are encapsulated by specific types for registration).

```csharp
options.ConfigureProfiles(profile => profile.AddTypeFormatter<CategoryName>
    (new CategoryNameFormatter()));
```

