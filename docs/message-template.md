# Message template renderer

### Overview

Renders the log event message with structured log value substitutions.

```
Template: {Message}
```

### Options

Rendering is further controlled by the `MessageTemplateRenderer.Options` type. The following properties are available:

|Property|Description|
|---|---|
|`TypeStyles`|A dictionary of markup styles that are associated to specific types. The renderer evaluates the type of value being rendered and looks for a style in this dictionary. If found, the markup is applied before the value is rendered and the tag is closed afterward.|
|`DefaultTypeStyle`|A single markup style used as a fallback for all values rendered whose types are not located in the `TypeSyles` dictionary.|
|`ValueStyles`|A dictionary of markup styles that are associated to specific values. The renderer evaluates the value being rendered and looks for a style in this dictionary. If found, the markup is applied before the value is rendered and the tag is closed afterward.|
|`TypeFormatters`|A dictionary of formatting functions that are associated to specific types. The renderer evaluates the type of value being rendered and looks for a formatting function in this dictionary. If found, the function provides the value to render.
|`DefaultTypeFormatter`|A formatting function used as a fallback for all values rendered whose types are not located in the `TypeFormatters` dictionary.

Instead of accessing the dictionaries directly, alternatively use the extension methods on the `FormattedLogValuesRenderer.Options` types to set formatters and styles as shown below:

|Extension Method|Description|
|---|---|
|`AddTypeFormatter(Type, Func<>)`|Adds a formatting function for the given type|
|`AddTypeFormatter(IEnumerable<Type>, Func<>)`|Adds a formatting function for the given types|
|`AddTypeFormatter<T>(Func<>)`|Adds a formatting function for the type identified by the generic parameter|
|`AddTypeStyle(Type, string)`|Adds a style for the given type|
|`AddTypeStyle(IEnumerable<Type>, string)`|Adds a style for the given types|
|`AddTypestyle<T>(string)`|Adds a style for the type identified by the generic parameter|
|`AddValueStyle<T>(T, string)`|Adds a style for a specific value|
|`AddValueStyle(IEnumerable<object>, string)`|Adds a style for the given specific values|
|`ClearTypeFormatters()`|Clears all type formatters|
|`ClearTypeStyles()`|Clears all type styles|
|`ClearValueStyles()`|Clears all value styles|

