using System;
using System.Reflection;
using Vertical.SpectreLogger.Core;
using Vertical.SpectreLogger.Reflection;

namespace Vertical.SpectreLogger.Internal
{
    /// <summary>
    /// Represents the descriptor of a <see cref="ITemplateRenderer"/>
    /// </summary>
    public class RendererDescriptor : IEquatable<RendererDescriptor>
    {
        internal RendererDescriptor(Type implementationType)
        {
            ImplementationType = implementationType ?? throw new ArgumentNullException(nameof(implementationType));
            Template = implementationType.GetCustomAttribute<TemplateAttribute>()?.Template 
                       ??
                       throw new ArgumentException($"Type '{implementationType}' does not have a template defined (use TemplateAttribute)");

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
        public bool Equals(RendererDescriptor? other) => ImplementationType == other?.ImplementationType && Template == other.Template;

        /// <inheritdoc />
        public override bool Equals(object? obj) => obj is RendererDescriptor other && Equals(other);

        /// <inheritdoc />
        public override int GetHashCode() => HashCode.Combine(ImplementationType, Template);
    }
}