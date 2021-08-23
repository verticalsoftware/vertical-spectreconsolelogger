using System;
using System.Linq;
using System.Reflection;

namespace Vertical.SpectreLogger.Infrastructure
{
    internal static class DynamicActivator
    {
        internal static object CreateInstance(Type type, object[] args)
        {
            var constructors = type.GetConstructors();

            foreach (var constructor in constructors)
            {
                if (TryCreateInstance(constructor, args, out var instance))
                {
                    return instance!;
                }
            }

            throw new InvalidOperationException(
                $"Could not find compatible constructor for type {type} using arguments " + 
                $" ({(string.Join(",", args.Select(arg => arg.GetType().Name)))})");
        }

        private static bool TryCreateInstance(ConstructorInfo constructor, object[] args, out object? obj)
        {
            var parameters = constructor.GetParameters();
            var orderedArguments = new object[parameters.Length];
            var assignIndex = 0;

            foreach (var parameter in parameters)
            {
                var parameterValue = args.FirstOrDefault(arg => parameter.ParameterType.IsAssignableFrom(arg.GetType()));

                if (parameterValue == null)
                {
                    obj = default;
                    return false;
                }
                
                orderedArguments[assignIndex++] = parameterValue;
            }

            obj = constructor.Invoke(orderedArguments);
            return true;
        }
    }
}