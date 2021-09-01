using System;
using System.Linq;

namespace Vertical.SpectreLogger.Core
{
    /// <summary>
    /// Defines the template for a rendering component.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TemplateAttribute : Attribute
    {
        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        /// <param name="template">Template value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="template"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="template"/> is whitespace</exception>
        public TemplateAttribute(string template)
        {
            Template = template ?? throw new ArgumentNullException(nameof(template));

            if (template.All(char.IsWhiteSpace))
            {
                throw new ArgumentException("Template cannot be empty/whitespace");
            }
        }

        /// <summary>
        /// Gets the template value.
        /// </summary>
        public string Template { get; }

        /// <inheritdoc />
        public override string ToString() => Template;

        /// <inheritdoc />
        public override int GetHashCode() => Template.GetHashCode();
    }
}