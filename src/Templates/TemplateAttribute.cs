using System;
using System.Linq;
using System.Reflection;

namespace Vertical.SpectreLogger.Templates
{
    /// <summary>
    /// Defines the template for a rendering component.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
    public class TemplateAttribute : Attribute
    {
        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        /// <param name="template">Template value when applied to a class.</param>
        public TemplateAttribute(string? template = null)
        {
            Template = template;
        }

        /// <summary>
        /// Gets the template value.
        /// </summary>
        public string? Template { get; }

        /// <inheritdoc />
        public override string ToString() => Template ?? "(dynamic)";

        /// <summary>
        /// Attempts to locate an instance of the attribute from the given type.
        /// </summary>
        /// <param name="type">Type that is decorated with the attribute.</param>
        /// <returns><see cref="TemplateAttribute"/> or null.</returns>
        internal static string? ValueFromType(Type type)
        {
            var instance = type.GetCustomAttribute<TemplateAttribute>();

            if (instance != null)
            {
                return instance.Template
                       ??
                       throw new InvalidOperationException($"[Template] attribute applied to {type} type, "
                                                           + "but no template was provided.");
            }

            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Static;

            var templateProperty = type
                .GetProperties(bindingFlags)
                .FirstOrDefault(property => property.GetCustomAttribute<TemplateAttribute>() != null);

            if (templateProperty != null)
            {
                return templateProperty.GetValue(null /* static */) as string
                       ?? throw new InvalidOperationException(
                           $"[Template] attribute applied to {type}.{templateProperty.Name} " +
                           "but the property returned null.");
            }

            var templateField = type
                .GetFields(bindingFlags)
                .FirstOrDefault(field => field.GetCustomAttribute<TemplateAttribute>() != null);

            if (templateField != null)
            {
                return templateField.GetValue(null /* static */) as string
                       ?? throw new InvalidOperationException(
                           $"[Template] attribute applied to {type}.{templateField.Name} " +
                           "but the property returned null.");
            }

            return null;
        }
    }
}