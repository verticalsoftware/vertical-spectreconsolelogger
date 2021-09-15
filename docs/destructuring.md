# Destructured Output

## Overview

Destructuring is decomposing a complex object for output. This involves printing properties of an object, or printing the items of a collection. When using structured log messages, if you proceed a complex type with `@`, the object will be printed in JSON-ish notation. When printing the object, the logging provider will obey formatting and styling rules for each value.

### Example

```csharp
public record Person(int Id, string Name);

// ..

var person = new Person(10, "vertical-dev");

logger.LogInformation("Record saved, item={person}", person);
logger.LogInformation("Record saved, destructured item={@person}", person);

// Output
// Record saved, item=Vertical.Example.Person
// Record saved, destructured item={Id=10, Name=vertical-dev}
```

### Controlling Destructuring

You can control certain aspects of destructuring by configuring the `DestructuringOptions` type. The following properties are available:

|Property|Description|
|---|---|
|MaxDepth|Gets/sets how many times the writer will recursively descend into the child properties of an object.|
|MaxCollectionItems|Gets/sets the maximum number of items to display in a collection.|
|MaxProperties|Gets/sets the maximum number of properties to display of an object.|

### Example

```csharp
config.ConfigureProfiles(profile => profile.ConfigureOptions<DestructuringOptions>(
    destructuring => destructuring.MaxDepth = 3));
```