using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Vertical.SpectreLogger.Core;

namespace Vertical.SpectreLogger.Reflection
{
    internal static class TypeActivator
    {
        /// <summary>
        /// Verifies a type can be dynamically activated.
        /// </summary>
        /// <param name="implementationType">Implementation type.</param>
        /// <param name="reason">The reason the type cannot be created.</param>
        /// <typeparam name="TService">Service type.</typeparam>
        /// <exception cref="ArgumentNullException"><paramref name="implementationType"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="implementationType"/> is abstract, an interface,
        /// or does not have a compatible constructor.</exception>
        internal static bool CanCreateInstanceOfType<TService>(Type implementationType, out string? reason)
        {
            if (implementationType == null)
            {
                 throw new ArgumentNullException(nameof(implementationType));
            }

            if (!typeof(TService).IsAssignableFrom(implementationType))
            {
                reason = $"Type '{implementationType}' is not assignable as {typeof(ITemplateRenderer)}";
                return false;
            }

            if (implementationType.IsAbstract || implementationType.IsInterface)
            {
                reason = $"Type '{implementationType}' is abstract and cannot be activated";
                return false;
            }

            if (!implementationType.GetConstructors().Any())
            {
                reason = $"Type '{implementationType}' has no public constructors.";
                return false;
            }

            reason = null;
            return true;
        }
        
        /// <summary>
        /// Tries to create an instance of the given type.
        /// </summary>
        /// <param name="dependencies">The dependencies to use as parameter arguments.</param>
        /// <returns>The object instance.</returns>
        /// <typeparam name="T">The type of object to create.</typeparam>
        /// <exception cref="ArgumentNullException"><paramref name="dependencies"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="dependencies"/> contains a null reference.</exception>
        /// <exception cref="InvalidOperationException">A suitable constructor was not matched.</exception>
        /// <remarks>
        /// This method returns the constructor with the highest number of matching arguments.
        /// </remarks>
        internal static T CreateInstance<T>(List<object> dependencies)
        {
            return (T) CreateInstance(typeof(T), dependencies);
        }
        
        /// <summary>
        /// Tries to create an instance of the given type.
        /// </summary>
        /// <param name="type">The type to create.</param>
        /// <param name="dependencies">The dependencies to use as parameter arguments.</param>
        /// <returns>The object instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="dependencies"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="dependencies"/> contains a null reference.</exception>
        /// <exception cref="InvalidOperationException">A suitable constructor was not matched.</exception>
        /// <remarks>
        /// This method returns the constructor with the highest number of matching arguments.
        /// </remarks>
        internal static object CreateInstance(Type type, List<object> dependencies)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (dependencies == null)
            {
                throw new ArgumentNullException(nameof(dependencies));
            }

            if (dependencies.Any(dep => dep == null))
            {
                throw new ArgumentException("Dependencies cannot contain null values.");
            }

            var constructors = type.GetConstructors().OrderByDescending(ctor => ctor.GetParameters().Length);
            var parameterArray = new List<object>(dependencies.Count);

            foreach (var constructor in constructors)
            {
                parameterArray.Clear();

                if (!TryMatchConstructorArguments(constructor, dependencies, parameterArray))
                    continue;
                
                return constructor.Invoke(parameterArray.ToArray());
            }

            var message =
                $"Could not find a compatible constructor for type {type} using the following dependencies:"
                + Environment.NewLine
                + string.Join(Environment.NewLine, dependencies.Select(dep => $"  {dep.GetType()}"));

            throw new InvalidOperationException(message);
        }

        private static bool TryMatchConstructorArguments(
            MethodBase constructor,
            List<object> dependencies,
            ICollection<object> parameterArray)
        {
            foreach (var parameter in constructor.GetParameters())
            {
                var parameterType = parameter.ParameterType;
                var dependency = dependencies.FirstOrDefault(obj => parameterType.IsInstanceOfType(obj));

                if (dependency == null)
                    return false;
                
                parameterArray.Add(dependency);
            }

            return true;
        }
    }
}