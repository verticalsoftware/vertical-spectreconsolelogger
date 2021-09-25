# Styling

## Overview

Like formatting, you can style values and types of values any way you want on an event-severity basis.

### Styling values

Applying markup to values being rendered is configured by calling the `AddValueStyle` and `AddTypeStyle` methods on the `LogLevelProfile` object. When specifying markup, ensure it is enclosed in square brackets like you would using the Spectre Console otherwise. The logger will print closing tags automatically. 

Examples:

```csharp

// Print all strings in orange
config.ConfigureProfiles(profile => profile.AddTypeStyle<string>("[orange3]"));

// Print all strings in orange in Debug events only
config.ConfigureProfile(LogLevel.Debug, profile => profile.AddTyleStyle<string>("[orange3"));

// Print all numbers in magenta (see the Types class for more groups)
config.ConfigureProfiles(profile => profile.AddTypeStyle(Types.Numerics, "[magenta1]")); 

// Print boolean values in green and red
config.ConfigureProfiles(profile => profile.AddValueStyle(false, "[red1]"));
config.ConfigureProfiles(profile => profile.AddValueStyle(true, "[palegreen3]"));

```

The library defines several `Type` arrays that can be used to apply styling to several types at once.

|Property|Description|
|---|---|
|`Types.Characters`|`char`, `char?`, `string`|,
|`Types.Pointers`|`IntPtr`, `UIntPtr` + their `Nullable` variants
|`Types.RealNumbers`|`float`, `double`, `decimal` + their `Nullable` variants.|
|`Types.SignedIntegers`|`sbyte`, `uhort`, `int`, `long` + their `Nullable` variants.|
|`Types.Temporal`|`DateTime`, `DateTimeOffset`, `TimeSpan` + their `Nullable` variants.|
|`Types.UnsignedIntegers`|`byte`, `ushort`, `uint`, `ulong` + their `Nullable` variants.|

## See Also

- [(Next) Destructured Output](./destructuring.md)
- [Formatting Log Values](./formatting.md)
