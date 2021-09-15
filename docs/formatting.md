# Value Formatting

## Overview

Log values can be formatted before they are output to the console. The easiest out-of-box way to format values is to leverage .Net [composite formatting](https://docs.microsoft.com/en-us/dotnet/standard/base-types/composite-formatting) syntax directly in output templates and structured log messages. Custom formatting can be achieved by registering [ICustomFormatter](https://docs.microsoft.com/en-us/dotnet/api/system.icustomformatter?view=net-5.0) instances and associating them with specific types.

### Composite Formatting

#### In Output Templates

Using composite formatting in output templates require support from the specific renderer being used. For instance, the `DateTimeRenderer` allows both alignment and formatting using standard .Net syntax (for example, `{DateTime:s}` will output the value using the sortable pattern).

Most renderers support the alignment parameter, while some support the format string.

#### In Log Messages

Formatting in structured log messages is applied by using the composite format notation. For instance, consider the following:

```csharp
logger.LogInformation("Processed transaction at {date:f} for {amount:C}",
    DateTime.Now,
    100m);
    
// Output: Processed transaction at Monday, June 15, 2009 1:45:30 PM for $100.00
```

### Custom Formatting

Imagine a type called `Customer` that is located in a 3rd party library and contains sensitive data that cannot be displayed in destructured output, and the `ToString` method cannot be modified. The obvious way to solve this problem is to log a new object that is a subset of the `Customer` type like so:

```csharp
public async Task SaveCustomerAsync(Customer customer)
{
    await database.SaveAsync(customer);
    
    logger.LogInformation("Created new customer record {customer}",
        new { Id = customer.Id, Name = customer.Name });
}    
```

This works, but must be repeated every time a `Customer` needs to be logged. A consistent mechanism for formatting the `Customer` type for logging would be to provide a `ICustomFormatter` instance and register it with the logging provider. 

```csharp
public class CustomerFormatter : ICustomFormatter
{
    public override string Format(string? format, object? arg, IFormatProvider? provider)
    {
        var customer = (Customer)arg;
            
        return $"Id={customer.Id}, Name={customer.Name}";
    }
}

// Register the formatter during configuration.

config.ConfigureProfiles(profile => profile.AddTypeFormatter<Customer>(new CustomerFormatter()));
```

There are additional tools to make custom formatting less burdensome. First, instead of having to define an implementation of `ICustomFormatter`, you can register a delegate instead. The following example produces the same behavior

```csharp
// Using a formatting delegate - note no additional registration
// is necessary.

config.ConfigureProfiles(profile => profile.AddTypeFormatter<Customer>((format, object, IFormatProvider) => 
{
    var customer = (Customer)arg;            
    return $"Id={customer.Id}, Name={customer.Name}";
});
```

Secondly, you may decorate `ICustomFormatter` implementations with the `TypeFormatter` attribute and call a single method to register them all with assembly scanning.

```csharp
[TypeFormatter(typeof(Customer))]
public class CustomerFormatter : ICustomFormatter
{
    // ...
}

// Registration

config.ConfigureProfiles(profile => profile.AddTypeFormatters());
```

> ⚠️Important
>
> Make sure your type can be created by activation (e.g. has a parameterless public default constructor).

> 💡 Note
> 
> Since `ICustomFormatter` is being used, you can also extend the functionality with format strings - see [Composite Formatting](https://docs.microsoft.com/en-us/dotnet/api/system.icustomformatter?view=net-5.0).