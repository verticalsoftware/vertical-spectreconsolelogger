# New line renderer

### Overview

Inserts a new line to the output and optionally offsets or sets the margin.

```
Template: {NewLine[?][:[-]<margin>[!]]}
```

### Options

> 💡 Note
>
> Renderer names and options within the template are case-sensitive.

|Template options|Description|
|---|---|
|`?`|Inserts a newline character only if the write buffer is not already positioned on a new line at the currently set margin.|
|`-`|Offsets the margin to the left by `<margin>` spaces. If omitted and the `!` option is not used, the margin is offset to right.
|`<margin>`|The value to either offset or assign.|
|`!`|Sets the margin to the given value without offsetting.|



