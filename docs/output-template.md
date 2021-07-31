# vertical-spectreconsolelogger - The output template

## Overview

The output template controls the composite structure of each log event. It contains the name of different rendering components between handlebars. An example of an output template is:

```
{LogLevel}: {CategoryName}{NewLine:4}{Message}{Exception:NewLine}
```

This would render log events similarly to the Microsoft Console logger:
