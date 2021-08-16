using System;
using System.Linq;

namespace Vertical.SpectreLogger.Core
{
    /// <summary>
    /// Used to decorate renderers with the template pattern.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TemplateAttribute : Attribute
    {
        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        /// <param name="templatePattern">The template matching pattern.</param>
        /// <exception cref="ArgumentNullException"><paramref name="templatePattern"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="templatePattern"/> is whitespace.</exception>
        public TemplateAttribute(string templatePattern)
        {
            TemplatePattern = templatePattern ?? throw new ArgumentNullException(nameof(templatePattern));

            if (templatePattern.All(char.IsWhiteSpace))
            {
                throw new ArgumentException("Template pattern cannot be empty");
            }
        }

        /// <summary>
        /// Gets the template pattern.
        /// </summary>
        public string TemplatePattern { get; }

        /// <inheritdoc />
        public override string ToString() => TemplatePattern;
    }
}