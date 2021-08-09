# Scope values renderer

### Overview

Renders any scope values available in a log event.

```
Template: {ScopeValues[:NewLine[?][:[-]<margin>[!]]]}
```

### Options

|Template Option|Description|
|---|---|
|`[:NewLine]`|Writes a newline character to the output before writing the exception.|
|`?`|Inserts a newline character only if the write buffer is not already positioned on a new line at the currently set margin.|
|`-`|Offsets the margin to the left by `<margin>` spaces. If omitted and the `!` option is not used, the margin is offset to right.
|`<margin>`|The number of spaces to offset the current margin, or the exact value to set the margin to (when the `!` option is used)|
|`!`|Sets the margin to the given value without offsetting.|

Rendering is further controlled by configuring the `ScopeValuesRenderer.Options` type that has the following properties:

|Property|Description|
|---|---|
|`DefaultTypeFormatter`|A formatting function used as a fallback for all values rendered whose types are not located in the `TypeFormatters` dictionary.
|`DefaultTypeStyle`|A single markup style used as a fallback for all values rendered whose types are not located in the `TypeSyles` dictionary.|
|`ProviderFilter`|A function that is given and returns an `IEnumerable<object?>` collection of scope values. This is useful for applying any filtering, limiting, transformations, etc. to the scope values before rendering.|
|`Separator`|A string that is inserted between each scope value - defaults to ` => `|
|`TypeFormatters`|A dictionary of formatting functions that are associated to specific types. The renderer evaluates the type of value being rendered and looks for a formatting function in this dictionary. If found, the function provides the value to render.
|`TypeStyles`|A dictionary of markup styles that are associated to specific types. The renderer evaluates the type of value being rendered and looks for a style in this dictionary. If found, the markup is applied before the value is rendered and the tag is closed afterward.|
|`ValueStyles`|A dictionary of markup styles that are associated to specific values. The renderer evaluates the value being rendered and looks for a style in this dictionary. If found, the markup is applied before the value is rendered and the tag is closed afterward.|

Instead of accessing the dictionaries directly, alternatively use the extension methods on the `MultiTypeRenderingOptions` type to set formatters and styles as shown below:

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