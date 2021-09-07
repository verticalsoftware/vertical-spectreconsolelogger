using System;
using System.Reflection;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Reflection;
using Vertical.SpectreLogger.Templates;

namespace Vertical.SpectreLogger.Internal
{
    /// <summary>
    /// Represents the descriptor of a <see cref="ITemplateRenderer"/>
    /// </summary>
    public class TemplateDescriptor : IEquatable<TemplateDescriptor>
    {
        internal TemplateDescriptor(Type implementationType)
        {
            ImplementationType = implementationType ?? throw new ArgumentNullException(nameof(implementationType));
            Template = GetTemplateValue(implementationType);

            if (TypeActivator.CanCreateInstanceOfType<ITemplateRenderer>(implementationType, out var reason))
                return;

            throw new ArgumentException(reason, nameof(implementationType));
        }
        
        /// <summary>
        /// Gets the implementation type.
        /// </summary>
        public Type ImplementationType { get; }
        
        /// <summary>
        /// Gets the template that is associated with the renderer.
        /// </summary>
        public string Template { get; }

        /// <inheritdoc />
        public override string ToString() => $"{ImplementationType}=\"{Template}\"";

        /// <inheritdoc />
        public bool Equals(TemplateDescriptor? other) => ImplementationType == other?.ImplementationType && Template == other.Template;

        /// <inheritdoc />
        public override bool Equals(object? obj) => obj is TemplateDescriptor other && Equals(other);

        /// <inheritdoc />
        public override int GetHashCode() => HashCode.Combine(ImplementationType, Template);

        private static string GetTemplateValue(Type type)
        {
            return
                TemplateAttribute.ValueFromType(type)
                ??
                throw new ArgumentException($"Type '{type}' does not have a template defined (use TemplateAttribute)");
        }
    }
}