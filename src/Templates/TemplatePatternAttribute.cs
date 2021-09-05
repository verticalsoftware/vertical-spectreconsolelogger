using System;
using System.Linq;
using System.Reflection;

namespace Vertical.SpectreLogger.Templates
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class TemplatePatternAttribute : Attribute
    {
        /// <summary>
        /// Tries to get the value of a property or field decorated with this attribute.
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns>String value of the matching property or field; otherwise null.</returns>
        internal static string? ValueFromType(Type type)
        {
            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Static;

            var attributeValue =
                type.GetProperties(bindingFlags)
                    .FirstOrDefault(property => property.GetCustomAttribute<TemplatePatternAttribute>() != null
                                                && property.CanRead
                                                && property.PropertyType == typeof(string))?
                    .GetValue(null)
                ??
                type.GetFields(bindingFlags)
                    .FirstOrDefault(field => field.GetCustomAttribute<TemplatePatternAttribute>() != null
                                             && field.FieldType == typeof(string))?
                    .GetValue(null);

            return attributeValue as string;
        }
    }
}